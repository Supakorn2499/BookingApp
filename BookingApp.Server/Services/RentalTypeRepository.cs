using BookingApp.Server.Models;
using Dapper;
using Npgsql;


namespace BookingApp.Server.Services
{
    public class RentalTypeRepository
    {
        private readonly string _connectionString;

        public RentalTypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(RentalType rental_type)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                INSERT INTO rental_type (code, name1,name2, active,inactivedate,createby, createatutc) 
                VALUES (@code, @name1,@name2, @active,@inactivedate,@createby, @createatutc)
                RETURNING id;";

                if (rental_type.active == "Y")
                {
                    rental_type.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    code = rental_type.code,
                    name1 = rental_type.name1,
                    name2 = rental_type.name2,
                    active = rental_type.active,
                    inactivedate = rental_type.inactivedate,
                    createby = rental_type.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM rental_type WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(RentalType rental_type)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE rental_type 
                SET 
                    code= @code,
                    name1 = @name1,
                    name2 = @name2,
                    active = @active,
                    inactivedate = @inactivedate,
                    updateby=@updateby, 
                    updateatutc=@updateatutc
                WHERE id = @id;";
                if (rental_type.active == "Y")
                {
                    rental_type.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    id = rental_type.id,
                    code = rental_type.code,
                    name1 = rental_type.name1,
                    name2 = rental_type.name2,
                    active = rental_type.active,
                    inactivedate = rental_type.inactivedate,
                    updateby = rental_type.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<RentalType> RentalTypes, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM rental_type
            WHERE (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(name2) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword))
            ORDER BY id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            FROM rental_type
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
                    var rental_types = await multi.ReadAsync<RentalType>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (rental_types, totalRecords);
                }
            }
        }

        public async Task<RentalType?> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM rental_type
            WHERE id = @id;";

                return await connection.QueryFirstOrDefaultAsync<RentalType>(query, new { id = id });
            }
        }

    }
}
