using BookingApp.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;
using System.ComponentModel.Design;
using System;
using Dapper;

namespace BookingApp.Server.Services
{
    public class SaleTeamRepository
    {
        private readonly string _connectionString;

        public SaleTeamRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(SaleTeam saleTeam)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                INSERT INTO saleteam (companyid,code, name1,name2, active,inactivedate,createby, createatutc) 
                VALUES (@companyid,@code, @name1,@name2, @active,@inactivedate,@createby, @createatutc)
                RETURNING id;";

                if (saleTeam.active == "Y")
                {
                    saleTeam.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    companyid = saleTeam.companyid,
                    code = saleTeam.code,
                    name1 = saleTeam.name1,
                    name2 = saleTeam.name2,
                    active = saleTeam.active,
                    inactivedate = saleTeam.inactivedate,
                    createby = saleTeam.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM saleteam WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(SaleTeam saleTeam)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE saleteam 
                SET 
                    companyid = @companyid,
                    code= @code,
                    name1 = @name1,
                    name2 = @name2,
                    active = @active,
                    inactivedate = @inactivedate,
                    updateby=@updateby, 
                    updateatutc=@updateatutc
                WHERE id = @id;";
                if (saleTeam.active == "Y")
                {
                    saleTeam.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    id = saleTeam.id,
                    companyid = saleTeam.companyid,
                    code = saleTeam.code,
                    name1 = saleTeam.name1,
                    name2 = saleTeam.name2,
                    active = saleTeam.active,
                    inactivedate = saleTeam.inactivedate,
                    updateby = saleTeam.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<SaleTeam> SaleTeams, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM saleteam
            WHERE (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(name2) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword))
            ORDER BY id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            FROM saleteam
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
                    var saleteams = await multi.ReadAsync<SaleTeam>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (saleteams, totalRecords);
                }
            }
        }

        public async Task<SaleTeam?> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM saleteam
            WHERE id = @id;";

                return await connection.QueryFirstOrDefaultAsync<SaleTeam>(query, new { id = id });
            }
        }

    }
}
