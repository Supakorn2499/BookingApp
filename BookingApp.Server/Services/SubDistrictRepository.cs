using BookingApp.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;
using System.ComponentModel.Design;
using System;
using Dapper;

namespace BookingApp.Server.Services
{
    public class SubDistrictRepository
    {
        private readonly string _connectionString;

        public SubDistrictRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(SubDistrict district)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                INSERT INTO postalsubdistrict (code, name1,name2,provincecode,districtcode,postal_code,createby, createatutc) 
                VALUES (@code, @name1,@name2,@provincecode,@districtcode,@postal_code,@createby, @createatutc)
                RETURNING id;";

                var paraments = new
                {
                    code = district.code,
                    name1 = district.name1,
                    provincecode = district.provincecode,
                    districtcode = district.districtcode,
                    postal_code = district.postal_code,
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
                const string query = "DELETE FROM postalsubdistrict WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(SubDistrict district)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE postalsubdistrict 
                SET 
                    code= @code,
                    name1 = @name1,
                    name2 = @name2,
                    provincecode = @provincecode,
                    districtcode = @districtcode,
                    postal_code = @postal_code,
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
                    districtcode = district.districtcode,
                    postal_code = district.postal_code,
                    updateby = district.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<SubDistrict> SubDistricts, int TotalRecords)> GetAsync(string provinceCode, string districtCode, string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM postalsubdistrict
            WHERE provincecode =@provincecode and districtcode = @districtcode and (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(name2) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword))
            ORDER BY id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            FROM postalsubdistrict
            WHERE provincecode =@provincecode and districtcode = @districtcode and (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(name2) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword));";

                var offset = (pageNumber - 1) * pageSize;

                using (var multi = await connection.QueryMultipleAsync(query, new
                {
                    Keyword = string.IsNullOrWhiteSpace(keyword) ? null : $"%{keyword}%",
                    Offset = offset,
                    PageSize = pageSize,
                    provincecode = provinceCode,
                    districtcode = districtCode
                }))
                {
                    var subdistricts = await multi.ReadAsync<SubDistrict>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (subdistricts, totalRecords);
                }
            }
        }

        public async Task<SubDistrict?> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM postalsubdistrict
            WHERE id = @id;";

                return await connection.QueryFirstOrDefaultAsync<SubDistrict>(query, new { id = id });
            }
        }

    }
}
