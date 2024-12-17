using BookingApp.Server.Models;
using Dapper;
using Npgsql;

namespace BookingApp.Server.Services
{
    public class MeterRepository
    {
        private readonly string _connectionString;

        public MeterRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(Meter meter)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                INSERT INTO meter (code, name1,name2, active,price_per_unit,inactivedate,createby, createatutc) 
                VALUES (@code, @name1,@name2, @active,@price_per_unit,@inactivedate,@createby, @createatutc)
                RETURNING id;";

                if (meter.active == "Y")
                {
                    meter.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    code = meter.code,
                    name1 = meter.name1,
                    name2 = meter.name2,
                    active = meter.active,
                    price_per_unit = meter.price_per_unit,
                    inactivedate = meter.inactivedate,
                    createby = meter.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM meter WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(Meter meter)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE meter 
                SET 
                    code= @code,
                    name1 = @name1,
                    name2 = @name2,
                    active = @active,
                    inactivedate = @inactivedate,
                    updateby=@updateby, 
                    updateatutc=@updateatutc
                WHERE id = @id;";
                if (meter.active == "Y")
                {
                    meter.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    id = meter.id,
                    code = meter.code,
                    name1 = meter.name1,
                    name2 = meter.name2,
                    active = meter.active,
                    inactivedate = meter.inactivedate,
                    updateby = meter.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<Meter> SaleTeams, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM meter
            WHERE (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(name2) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword))
            ORDER BY id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            FROM meter
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
                    var saleteams = await multi.ReadAsync<Meter>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (saleteams, totalRecords);
                }
            }
        }

        public async Task<Meter?> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM meter
            WHERE id = @id;";

                return await connection.QueryFirstOrDefaultAsync<Meter>(query, new { id = id });
            }
        }

    }

}