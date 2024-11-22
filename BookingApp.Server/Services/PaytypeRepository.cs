using BookingApp.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;
using System.ComponentModel.Design;
using System;
using Dapper;

namespace BookingApp.Server.Services
{
    public class PaytypeRepository
    {
        private readonly string _connectionString;

        public PaytypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(Paytype paytype)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                INSERT INTO paytype (code, name1,name2,isdeposit,iswithdraw, inout,active,inactivedate,createby, createatutc) 
                VALUES (@code, @name1,@name2,@isdeposit,@iswithdraw,@inout,@active,@inactivedate,@createby, @createatutc)
                RETURNING id;";

                if (paytype.active == "Y")
                {
                    paytype.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    code = paytype.code,
                    name1 = paytype.name1,
                    name2 = paytype.name2,
                    isdeposit = paytype.isdeposit,
                    iswithdraw = paytype.iswithdraw,
                    inout = paytype.inout,
                    active = paytype.active,
                    inactivedate = paytype.inactivedate,
                    createby = paytype.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM paytype WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(Paytype paytype)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE paytype 
                SET 
                    code = @code, 
                    name1 = @name1,
                    name2 = @name2,
                    isdeposit = @isdeposit,
                    iswithdraw = @iswithdraw,
                    inout = @inout,
                    active = @active,
                    inactivedate = @inactivedate,
                    updateby=@updateby, 
                    updateatutc=@updateatutc
                WHERE id = @id;";
                if (paytype.active == "Y")
                {
                    paytype.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {

                    code = paytype.code,
                    name1 = paytype.name1,
                    name2 = paytype.name2,
                    isdeposit = paytype.isdeposit,
                    iswithdraw = paytype.iswithdraw,
                    inout = paytype.inout,
                    active = paytype.active,
                    inactivedate = paytype.inactivedate,
                    updateby = paytype.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<Paytype> Paytypes, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM paytype
            WHERE (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(name2) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword))
            ORDER BY id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            FROM paytype
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
                    var Paytypes = await multi.ReadAsync<Paytype>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (Paytypes, totalRecords);
                }
            }
        }

        public async Task<Paytype?> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM paytype
            WHERE id = @id;";

                return await connection.QueryFirstOrDefaultAsync<Paytype>(query, new { id = id });
            }
        }

    }
}
