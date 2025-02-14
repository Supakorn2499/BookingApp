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
                INSERT INTO meter (meter_type_id, meter_number,meter_value, status,active,inactivedate,createby, createatutc) 
                VALUES (@meter_type_id, @meter_number,@meter_value, @status,@active,@inactivedate,@createby, @createatutc)
                RETURNING id;";

                if (meter.active == "Y")
                {
                    meter.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    meter_type_id = meter.meter_type_id,
                    meter_number = meter.meter_number,
                    meter_value = meter.meter_value,
                    active = meter.active,
                    status = meter.status,
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
                    meter_type_id= @meter_type_id,
                    meter_number = @meter_number,
                    meter_value = @meter_value,
                    active = @active,
                    status = @status,
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
                    meter_type_id = meter.meter_type_id,
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