using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using BookingApp.Server.Models;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;

namespace BookingApp.Server.Services
{
    public class ProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddProductAsync(Product product)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                INSERT INTO product (companyid,productgroupid, umid,code, status, 
                active, inactivedate, name1,name2,stdprice,stdcost,minprice,remark1,createby, createatutc) 
                VALUES (@companyid,@productgroupid, @umid,@code, @status, 
                @active, @inactivedate, @name1,@name2,@stdprice,@stdcost,@minprice,@remark1,@createby, @createatutc)
                RETURNING id;";

                var paraments = new
                {
                    companyid = 1,
                    productgroupid = product.productgroupid,
                    umid = product.umid,
                    code = product.code,
                    status = product.status,
                    active = product.active,
                    inactivedate = DateTimeHelper.ConvertToUtc(product.inactivedate),
                    name1 = product.name1,
                    name2 = product.name2,
                    stdprice = product.stdprice,
                    stdcost = product.stdcost,
                    minprice = product.minprice,
                    remark1 = product.remark1,
                    createby = product.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM product WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE product 
                SET productgroupid=@productgroupid, 
                    umid=@umid,
                    code=@code, 
                    status=@status, 
                    active=@active, 
                    inactivedate=@inactivedate, 
                    name1=@name1,
                    name2=@name1,
                    stdprice=@stdprice,
                    remark1=@remark1,
                    updateby = @updateby, 
                    updateatutc = @updateatutc
                WHERE id = @id;";

                var paraments = new
                {
                    id = product.id,
                    productgroupid = product.productgroupid,
                    umid = product.umid,
                    code = product.code,
                    status = product.status,
                    active = product.active,
                    inactivedate = DateTimeHelper.ConvertToUtc(product.inactivedate),
                    name1 = product.name1,
                    name2 = product.name2,
                    stdprice = product.stdprice,
                    remark1 = product.remark1,
                    updateby = product.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<Product> Products, int TotalRecords)> GetProductsAsync(string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM product
            WHERE (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(name2) LIKE LOWER(@Keyword) OR
                   LOWER(code) LIKE LOWER(@Keyword))
            ORDER BY id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            FROM product
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
                    var products = await multi.ReadAsync<Product>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (products, totalRecords);
                }
            }
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM product
            WHERE id = @Id;";

                return await connection.QueryFirstOrDefaultAsync<Product>(query, new { Id = id });
            }
        }

    }

}
