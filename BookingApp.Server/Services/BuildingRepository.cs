using BookingApp.Server.Models;
using Dapper;
using Npgsql;

namespace BookingApp.Server.Services
{
    public class BuildingRepository
    {
        private readonly string _connectionString;

        public BuildingRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(Building building)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                INSERT INTO building (code, name1,name2, active,inactivedate,createby, createatutc) 
                VALUES (@code, @name1,@name2, @active,@inactivedate,@createby, @createatutc)
                RETURNING id;";

                if (building.active == "Y")
                {
                    building.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    code = building.code,
                    name1 = building.name1,
                    name2 = building.name2,
                    active = building.active,
                    inactivedate = building.inactivedate,
                    createby = building.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM building WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(Building building)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE building 
                SET 
                    code= @code,
                    name1 = @name1,
                    name2 = @name2,
                    active = @active,
                    inactivedate = @inactivedate,
                    updateby=@updateby, 
                    updateatutc=@updateatutc
                WHERE id = @id;";
                if (building.active == "Y")
                {
                    building.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    id = building.id,
                    code = building.code,
                    name1 = building.name1,
                    name2 = building.name2,
                    active = building.active,
                    inactivedate = building.inactivedate,
                    updateby = building.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<Building> SaleTeams, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM building
            WHERE (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(name2) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword))
            ORDER BY id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            FROM building
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
                    var buildings = await multi.ReadAsync<Building>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (buildings, totalRecords);
                }
            }
        }

        public async Task<Building?> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM building
            WHERE id = @id;";

                return await connection.QueryFirstOrDefaultAsync<Building>(query, new { id = id });
            }
        }

    }
}
