using BookingApp.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;
using System.ComponentModel.Design;
using System;
using Dapper;

namespace BookingApp.Server.Services
{
    public class ProvinceRepository
    {
        private readonly string _connectionString;

        public ProvinceRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(Province province)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                INSERT INTO postalprovince (code, name1,name2,createby, createatutc) 
                VALUES (@code, @name1,@name2,@createby, @createatutc)
                RETURNING id;";

                var paraments = new
                {
                    code = province.code,
                    name1 = province.name1,
                    name2 = province.name2,
                    createby = province.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM postalprovince WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(Province province)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE postalprovince 
                SET 
                    code= @code,
                    name1 = @name1,
                    name2 = @name2,
                    updateby=@updateby, 
                    updateatutc=@updateatutc
                WHERE id = @id;";

                var paraments = new
                {
                    id = province.id,
                    code = province.code,
                    name1 = province.name1,
                    name2 = province.name2,
                    updateby = province.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<Province> Provinces, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM postalprovince
            WHERE (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(name2) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword))
            ORDER BY id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            FROM postalprovince
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
                    var provinces = await multi.ReadAsync<Province>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (provinces, totalRecords);
                }
            }
        }

        public async Task<Province?> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM postalprovince
            WHERE id = @id;";

                return await connection.QueryFirstOrDefaultAsync<Province>(query, new { id = id });
            }
        }

    }
}
