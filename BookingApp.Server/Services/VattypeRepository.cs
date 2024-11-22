using BookingApp.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;
using System.ComponentModel.Design;
using System;
using Dapper;

namespace BookingApp.Server.Services
{
    public class VattypeRepository
    {
        private readonly string _connectionString;

        public VattypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(Vattype vattype)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                INSERT INTO vattype (code, name1,name2,rate,vat_type, active,inactivedate,createby, createatutc) 
                VALUES (@code, @name1,@name2,@rate,@vat_type, @active,@inactivedate,@createby, @createatutc)
                RETURNING id;";

                if (vattype.active == "Y")
                {
                   vattype.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    code = vattype.code,
                    name1 = vattype.name1,
                    name2 = vattype.name2,
                    rate = vattype.rate,
                    vat_type = vattype.vat_type,
                    active = vattype.active,
                    inactivedate = vattype.inactivedate,
                    createby = vattype.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM vattype WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(Vattype vattype)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE vattype 
                SET 
                    code= @code,
                    name1 = @name1,
                    name2 = @name2,
                    rate = @rate,
                    vat_type = @vat_type,
                    active = @active,
                    inactivedate = @inactivedate,
                    updateby=@updateby, 
                    updateatutc=@updateatutc
                WHERE id = @id;";
                if (vattype.active == "Y")
                {
                    vattype.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
         
                    code = vattype.code,
                    name1 = vattype.name1,
                    name2 = vattype.name2,
                    active = vattype.active,
                    rate = vattype.rate,
                    vat_type = vattype.vat_type,
                    inactivedate = vattype.inactivedate,
                    updateby = vattype.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<Vattype> Vattype, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM vattype
            WHERE (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(name2) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword))
            ORDER BY id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            FROM vattype
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
                    var vattype = await multi.ReadAsync<Vattype>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (vattype, totalRecords);
                }
            }
        }

        public async Task<Vattype?> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM vattype
            WHERE id = @id;";

                return await connection.QueryFirstOrDefaultAsync<Vattype>(query, new { id = id });
            }
        }

    }
}
