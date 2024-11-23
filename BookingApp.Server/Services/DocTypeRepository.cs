using BookingApp.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;
using System.ComponentModel.Design;
using System;
using Dapper;

namespace BookingApp.Server.Services
{
    public class DocTypeRepository
    {
        private readonly string _connectionString;

        public DocTypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(DocType doctype)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                INSERT INTO doctype (
                    system, menu, type, doc_group, code,
                    name1, name2, vat, cash, service,
                    tax_refund, createby, createatutc, stock, calflag, stockseq
                )
                VALUES (
                    @system, @menu, @type, @doc_group, @code,
                    @name1, @name2, @vat, @cash, @service,
                    @tax_refund, @createby, @createatutc, @stock, @calflag, @stockseq
                )
                RETURNING id;";

                var paraments = new
                {
                    system = doctype.system,
                    menu = doctype.menu,
                    type = doctype.type,
                    doc_group = doctype.doc_group,
                    code = doctype.code,
                    name1 = doctype.name1,
                    name2 = doctype.name2,
                    vat = doctype.vat,
                    cash = doctype.cash,
                    service = doctype.service,
                    tax_refund = doctype.tax_refund,
                    stock = doctype.stock,
                    calflag = doctype.calflag,
                    stockseq = doctype.stockseq,
                    createby = doctype.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM doctype WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(DocType doctype)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE doctype 
                SET 
                    system = @system,
                    menu = @menu,
                    type = @type,
                    doc_group = @doc_group,
                    code = @code,
                    name1 = @name1,
                    name2 = @name2,
                    vat = @vat,
                    cash = @cash,
                    service = @service,
                    tax_refund = @tax_refund,
                    updateby = @updateby,
                    updateatutc = @updateatutc,
                    stock = @stock,
                    calflag = @calflag,
                    stockseq = @stockseq
                WHERE id = @id;";

                var paraments = new
                {
                    id = doctype.id,
                    system = doctype.system,
                    menu = doctype.menu,
                    type = doctype.type,
                    doc_group = doctype.doc_group,
                    code = doctype.code,
                    name1 = doctype.name1,
                    name2 = doctype.name2,
                    vat = doctype.vat,
                    cash = doctype.cash,
                    service = doctype.service,
                    tax_refund = doctype.tax_refund,
                    stock = doctype.stock,
                    calflag = doctype.calflag,
                    stockseq = doctype.stockseq,
                    updateby = doctype.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<DocType> DocTypes, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM doctype
            WHERE (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(name2) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword))
            ORDER BY id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            FROM doctype
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
                    var doctypes = await multi.ReadAsync<DocType>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (doctypes, totalRecords);
                }
            }
        }

        public async Task<DocType?> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM doctype
            WHERE id = @id;";

                return await connection.QueryFirstOrDefaultAsync<DocType>(query, new { id = id });
            }
        }

    }
}
