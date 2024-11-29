using BookingApp.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;
using System.ComponentModel.Design;
using System;
using Dapper;

namespace BookingApp.Server.Services
{
    public class BankRepository
    {
        private readonly string _connectionString;

        public BankRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(Bank bank)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                INSERT INTO bank (companyid,code, name1,name2, active,inactivedate,botcode,swicfcode,createby, createatutc) 
                VALUES (@companyid,@code, @name1,@name2, @active,@inactivedate,@botcode,@swicfcode,@createby, @createatutc)
                RETURNING id;";

                if (bank.active == "Y")
                {
                    bank.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    companyid = bank.companyid,
                    code = bank.code,
                    name1 = bank.name1,
                    name2 = bank.name2,
                    active = bank.active,
                    inactivedate = bank.inactivedate,
                    createby = bank.createby,
                    botcode = bank.botcode,
                    swicfcode = bank.swicfcode,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM bank WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(Bank bank)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE bank 
                SET 
                    companyid = @companyid,
                    code= @code,
                    name1 = @name1,
                    name2 = @name2,
                    active = @active,
                    inactivedate = @inactivedate,
                    updateby=@updateby, 
                    updateatutc=@updateatutc,
                    botcode =@botcode,
                    swicfcode =@swicfcode
                WHERE id = @id;";
                if (bank.active == "Y")
                {
                    bank.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    id = bank.id,
                    companyid = bank.companyid,
                    code = bank.code,
                    name1 = bank.name1,
                    name2 = bank.name2,
                    active = bank.active,
                    inactivedate = bank.inactivedate,
                    updateby = bank.updateby,
                    botcode = bank.botcode,
                    swicfcode = bank.swicfcode,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<Bank> Banks, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM bank
            WHERE (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(name2) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword))
            ORDER BY id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            FROM bank
            WHERE (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(name2) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword));";

                var offset = (pageNumber - 1) * pageSize;

                using (var multi = await connection.QueryMultipleAsync(query, new
                {
                    Keyword = string.IsNullOrWhiteSpace(keyword) ? null : $"%{keyword}%",
                    Offset = offset,
                    PageSize = pageSize
                }))
                {
                    var banks = await multi.ReadAsync<Bank>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (banks, totalRecords);
                }
            }
        }

        public async Task<Bank?> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM bank
            WHERE id = @id;";

                return await connection.QueryFirstOrDefaultAsync<Bank>(query, new { id = id });
            }
        }

    }
}
