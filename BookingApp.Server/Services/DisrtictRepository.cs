using BookingApp.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;
using System.ComponentModel.Design;
using System;
using Dapper;

namespace BookingApp.Server.Services
{
    public class DisrtictRepository
    {
        private readonly string _connectionString;

        public DisrtictRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(District district)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                INSERT INTO postaldistrict (code, name1,name2,provincecode,createby, createatutc) 
                VALUES (@code, @name1,@name2,@provincecode,@createby, @createatutc)
                RETURNING id;";

                var paraments = new
                {
                    code = district.code,
                    name1 = district.name1,
                    provincecode = district.provincecode,
                    name2 = district.name2,
                    createby = district.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM postaldistrict WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(District district)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE postaldistrict 
                SET 
                    code= @code,
                    name1 = @name1,
                    name2 = @name2,
                    provincecode = @provincecode,
                    updateby=@updateby, 
                    updateatutc=@updateatutc
                WHERE id = @id;";

                var paraments = new
                {
                    id = district.id,
                    code = district.code,
                    name1 = district.name1,
                    name2 = district.name2,
                    provincecode = district.provincecode,
                    updateby = district.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<District> Districts, int TotalRecords)> GetAsync(string provinceCode, string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM postaldistrict
            WHERE provincecode =@provincecode and (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(name2) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword))
            ORDER BY id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            FROM postaldistrict
            WHERE provincecode =@provincecode and (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(name2) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword));";

                var offset = (pageNumber - 1) * pageSize;

                using (var multi = await connection.QueryMultipleAsync(query, new
                {
                    Keyword = string.IsNullOrWhiteSpace(keyword) ? null : $"%{keyword}%",
                    Offset = offset,
                    PageSize = pageSize,
                    provincecode = provinceCode
                }))
                {
                    var districts = await multi.ReadAsync<District>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (districts, totalRecords);
                }
            }
        }

        public async Task<District?> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM postaldistrict
            WHERE id = @id;";

                return await connection.QueryFirstOrDefaultAsync<District>(query, new { id = id });
            }
        }

    }
}
