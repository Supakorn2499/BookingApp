using BookingApp.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;
using System.ComponentModel.Design;
using System;
using Dapper;

namespace BookingApp.Server.Services
{
    public class BankBranchRepository
    {
        private readonly string _connectionString;

        public BankBranchRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(BankBranch bank)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                INSERT INTO bankbranch (companyid,bankid,code, name1,name2, active,inactivedate,createby, createatutc) 
                VALUES (@companyid,@bankid,@code, @name1,@name2, @active,@inactivedate,@createby, @createatutc)
                RETURNING id;";

                if (bank.active == "Y")
                {
                    bank.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    companyid = bank.companyid,
                    bankid = bank.bankid,
                    code = bank.code,
                    name1 = bank.name1,
                    name2 = bank.name2,
                    active = bank.active,
                    inactivedate = bank.inactivedate,
                    createby = bank.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM bankbranch WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(BankBranch bank)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE bankbranch 
                SET 
                    companyid = @companyid,
                    bankid = @bankid,
                    code= @code,
                    name1 = @name1,
                    name2 = @name2,
                    active = @active,
                    inactivedate = @inactivedate,
                    updateby=@updateby, 
                    updateatutc=@updateatutc
                WHERE id = @id;";
                if (bank.active == "Y")
                {
                    bank.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    id = bank.id,
                    companyid = bank.companyid,
                    bankid = bank.id,
                    code = bank.code,
                    name1 = bank.name1,
                    name2 = bank.name2,
                    active = bank.active,
                    inactivedate = bank.inactivedate,
                    updateby = bank.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<BankBranch> BankBranchs, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT a.*,b.name1 as bankname  
            FROM bankbranch a inner join bank b on a.bankid=b.id
            WHERE (@Keyword IS NULL OR 
                   LOWER(b.name1) LIKE LOWER(@Keyword) OR
                   LOWER(a.name1) LIKE LOWER(@Keyword) OR 
                   LOWER(a.name2) LIKE LOWER(@Keyword) OR 
                   LOWER(a.code) LIKE LOWER(@Keyword))
            ORDER BY id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            FROM bankbranch
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
                    var banks = await multi.ReadAsync<BankBranch>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (banks, totalRecords);
                }
            }
        }

        public async Task<BankBranch?> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM bankbranch
            WHERE id = @id;";

                return await connection.QueryFirstOrDefaultAsync<BankBranch>(query, new { id = id });
            }
        }

    }
}
