using BookingApp.Server.Models;
using Dapper;
using Npgsql;

namespace BookingApp.Server.Services
{

    public class MeterTypeRepository
    {
        private readonly string _connectionString;

        public MeterTypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(MeterType meter_type)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                INSERT INTO meter_type (code, name1,name2, active,price_per_unit,inactivedate,createby, createatutc) 
                VALUES (@code, @name1,@name2, @active,@price_per_unit,@inactivedate,@createby, @createatutc)
                RETURNING id;";

                if (meter_type.active == "Y")
                {
                    meter_type.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    code = meter_type.code,
                    name1 = meter_type.name1,
                    name2 = meter_type.name2,
                    active = meter_type.active,
                    price_per_unit= meter_type.price_per_unit,
                    inactivedate = meter_type.inactivedate,
                    createby = meter_type.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM meter_type WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(MeterType meter_type)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE meter_type 
                SET 
                    code= @code,
                    name1 = @name1,
                    name2 = @name2,
                    active = @active,
                    inactivedate = @inactivedate,
                    updateby=@updateby, 
                    updateatutc=@updateatutc
                WHERE id = @id;";
                if (meter_type.active == "Y")
                {
                    meter_type.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    id = meter_type.id,
                    code = meter_type.code,
                    name1 = meter_type.name1,
                    name2 = meter_type.name2,
                    active = meter_type.active,
                    inactivedate = meter_type.inactivedate,
                    updateby = meter_type.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<MeterType> SaleTeams, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM meter_type
            WHERE (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(name2) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword))
            ORDER BY id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            FROM meter_type
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
                    var saleteams = await multi.ReadAsync<MeterType>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (saleteams, totalRecords);
                }
            }
        }

        public async Task<MeterType?> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM meter_type
            WHERE id = @id;";

                return await connection.QueryFirstOrDefaultAsync<MeterType>(query, new { id = id });
            }
        }

    }

}