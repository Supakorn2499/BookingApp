using BookingApp.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;
using System.ComponentModel.Design;
using System;
using Dapper;
namespace BookingApp.Server.Services
{
    public class ProductGroupRepository
    {
        private readonly string _connectionString;

        public ProductGroupRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(ProductGroup productGroup)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                INSERT INTO productgroup (companyid,code, name1,name2,active,inactivedate,createby, createatutc) 
                VALUES (@companyid,@code, @name1,@name2,@active,@inactivedate,@createby, @createatutc)
                RETURNING id;";

                if (productGroup.active == "Y")
                {
                    productGroup.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    companyid = productGroup.companyid,
                    code = productGroup.code,
                    name1 = productGroup.name1,
                    name2 = productGroup.name2,
                    active = productGroup.active,
                    inactivedate = productGroup.inactivedate,
                    createby = productGroup.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM productgroup WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(ProductGroup productGroup)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE productgroup 
                SET 
                    companyid = @companyid,
                    code = @code, 
                    name1 = @name1,
                    name2 = @name2,
                    active = @active,
                    inactivedate = @inactivedate,
                    updateby=@updateby, 
                    updateatutc=@updateatutc
                WHERE id = @id;";
                if (productGroup.active == "Y")
                {
                    productGroup.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    id = productGroup.id,
                    companyid = productGroup.companyid,
                    code = productGroup.code,
                    name1 = productGroup.name1,
                    name2 = productGroup.name2,
                    active = productGroup.active,
                    inactivedate = productGroup.inactivedate,
                    updateby = productGroup.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<ProductGroup> productGroups, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM productgroup
            WHERE (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(name2) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword))
            ORDER BY id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            FROM productgroup
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
                    var ProductGroups = await multi.ReadAsync<ProductGroup>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (ProductGroups, totalRecords);
                }
            }
        }

        public async Task<ProductGroup?> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM productgroup
            WHERE id = @id;";

                return await connection.QueryFirstOrDefaultAsync<ProductGroup>(query, new { id = id });
            }
        }

    }
}
