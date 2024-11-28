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
                INSERT INTO product (companyid,productgroupid,code, status, 
                active, inactivedate, name1,name2,saleprice,unitname,remark,vattype,prodtype,createby, createatutc) 
                VALUES (@companyid,@productgroupid,@code, @status, 
                @active, @inactivedate, @name1,@name2,@saleprice,@unitname,@remark,@vattype,@prodtype,@createby, @createatutc)
                RETURNING id;";

                var paraments = new
                {
                    companyid = 1,
                    productgroupid = product.productgroupid,
                    code = product.code,
                    status = product.status,
                    active = product.active,
                    inactivedate = DateTimeHelper.ConvertToUtc(product.inactivedate),
                    name1 = product.name1,
                    name2 = product.name2,
                    prodtype = product.prodtype,
                    saleprice = product.saleprice,
                    unitname = product.unitname,
                    remark = product.remark,
                    vattype = product.vattype,
                    createby = product.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };
                string sql = DapperHelper.DebugSql(query, paraments);
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
                    code=@code, 
                    status=@status, 
                    active=@active, 
                    inactivedate=@inactivedate, 
                    name1=@name1,
                    name2=@name1,
                    saleprice=@saleprice,
                    remark=@remark,
                    updateby = @updateby, 
                    updateatutc = @updateatutc,
                    prodtype = @prodtype,
                    vattype =@vattype,
                    unitname =@unitname
                WHERE id = @id;";

                var paraments = new
                {
                    id = product.id,
                    companyid = 1,
                    productgroupid = product.productgroupid,
                    code = product.code,
                    status = product.status,
                    active = product.active,
                    inactivedate = DateTimeHelper.ConvertToUtc(product.inactivedate),
                    name1 = product.name1,
                    name2 = product.name2,
                    prodtype = product.prodtype,
                    saleprice = product.saleprice,
                    unitname = product.unitname,
                    remark = product.remark,
                    vattype = product.vattype,
                    updateby = product.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };
                string sql = DapperHelper.DebugSql(query, paraments);
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
