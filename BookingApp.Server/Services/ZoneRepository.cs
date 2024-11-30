using BookingApp.Server.Models;
using Dapper;
using Npgsql;

namespace BookingApp.Server.Services
{
    public class ZoneRepository
    {
        private readonly string _connectionString;

        public ZoneRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(Zone zone)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                INSERT INTO zone (code, name1,name2, active,inactivedate,createby, createatutc) 
                VALUES (@code, @name1,@name2, @active,@inactivedate,@createby, @createatutc)
                RETURNING id;";

                if (zone.active == "Y")
                {
                    zone.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    code = zone.code,
                    name1 = zone.name1,
                    name2 = zone.name2,
                    active = zone.active,
                    inactivedate = zone.inactivedate,
                    createby = zone.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM zone WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(Zone zone)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE zone 
                SET 
                    code= @code,
                    name1 = @name1,
                    name2 = @name2,
                    active = @active,
                    inactivedate = @inactivedate,
                    updateby=@updateby, 
                    updateatutc=@updateatutc
                WHERE id = @id;";
                if (zone.active == "Y")
                {
                    zone.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    id = zone.id,
                    code = zone.code,
                    name1 = zone.name1,
                    name2 = zone.name2,
                    active = zone.active,
                    inactivedate = zone.inactivedate,
                    updateby = zone.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<Zone> SaleTeams, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM zone
            WHERE (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(name2) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword))
            ORDER BY id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            FROM zone
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
                    var saleteams = await multi.ReadAsync<Zone>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (saleteams, totalRecords);
                }
            }
        }

        public async Task<Zone?> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM zone
            WHERE id = @id;";

                return await connection.QueryFirstOrDefaultAsync<Zone>(query, new { id = id });
            }
        }

    }
}
