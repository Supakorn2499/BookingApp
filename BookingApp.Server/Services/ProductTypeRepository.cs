using BookingApp.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;
using System.ComponentModel.Design;
using System;
using Dapper;

namespace BookingApp.Server.Services
{
    public class ProductTypeRepository
    {
        private readonly string _connectionString;

        public ProductTypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(ProductType productType)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                INSERT INTO prodtype (code, name1,name2,active,inactivedate,flag,createby, createatutc) 
                VALUES (@code, @name1,@name2,@active,@inactivedate,@flag,@createby, @createatutc)
                RETURNING id;";

                if (productType.active == "Y")
                {
                    productType.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    code = productType.code,
                    name1 = productType.name1,
                    name2 = productType.name2,
                    flag = productType.flag,
                    active = productType.active,
                    inactivedate = productType.inactivedate,
                    createby = productType.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM prodtype WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(ProductType productType)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE prodtype 
                SET 
                    code = @code, 
                    name1 = @name1,
                    name2 = @name2,
                    flag = @flag,
                    active = @active,
                    inactivedate = @inactivedate,
                    updateby=@updateby, 
                    updateatutc=@updateatutc
                WHERE id = @id;";
                if (productType.active == "Y")
                {
                    productType.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    id = productType.id,
                    code = productType.code,
                    name1 = productType.name1,
                    name2 = productType.name2,
                    flag = productType.flag,
                    active = productType.active,
                    inactivedate = productType.inactivedate,
                    updateby = productType.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<ProductType> ProductType, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM prodtype
            WHERE (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(name2) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword))
            ORDER BY id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            FROM prodtype
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
                    var ProductTypes = await multi.ReadAsync<ProductType>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (ProductTypes, totalRecords);
                }
            }
        }

        public async Task<ProductType?> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM prodtype
            WHERE id = @id;";

                return await connection.QueryFirstOrDefaultAsync<ProductType>(query, new { id = id });
            }
        }

    }
}
