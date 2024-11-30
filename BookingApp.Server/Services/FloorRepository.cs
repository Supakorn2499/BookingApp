using BookingApp.Server.Models;
using Dapper;
using Npgsql;


namespace BookingApp.Server.Services
{
    public class FloorRepository
    {
        private readonly string _connectionString;

        public FloorRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(Floor floor)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                INSERT INTO floor (code, name1,name2, active,inactivedate,createby, createatutc) 
                VALUES (@code, @name1,@name2, @active,@inactivedate,@createby, @createatutc)
                RETURNING id;";

                if (floor.active == "Y")
                {
                    floor.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    code = floor.code,
                    name1 = floor.name1,
                    name2 = floor.name2,
                    active = floor.active,
                    inactivedate = floor.inactivedate,
                    createby = floor.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM floor WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(Floor floor)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE floor 
                SET 
                    code= @code,
                    name1 = @name1,
                    name2 = @name2,
                    active = @active,
                    inactivedate = @inactivedate,
                    updateby=@updateby, 
                    updateatutc=@updateatutc
                WHERE id = @id;";
                if (floor.active == "Y")
                {
                    floor.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    id = floor.id,
                    code = floor.code,
                    name1 = floor.name1,
                    name2 = floor.name2,
                    active = floor.active,
                    inactivedate = floor.inactivedate,
                    updateby = floor.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<Floor> SaleTeams, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM floor
            WHERE (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(name2) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword))
            ORDER BY id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            FROM floor
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
                    var Floors = await multi.ReadAsync<Floor>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (Floors, totalRecords);
                }
            }
        }

        public async Task<Floor?> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM floor
            WHERE id = @id;";

                return await connection.QueryFirstOrDefaultAsync<Floor>(query, new { id = id });
            }
        }

    }
}
