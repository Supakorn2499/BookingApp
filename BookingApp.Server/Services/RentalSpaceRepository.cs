using BookingApp.Server.Models;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;
using System;

namespace BookingApp.Server.Services
{
    public class RentalSpaceRepository
    {
        private readonly string _connectionString;

        public RentalSpaceRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(RentalSpace rental_space)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                INSERT INTO rental_space (room_name, 
                    room_size, 
                    monthly_price, 
                    daily_price, 
                    hourly_price, 
                    zone_id, 
                    building_id, 
                    floor_id, 
                    rental_type_id, 
                    createby, 
                    createatutc, 
                    active, 
                    inactivedate) 
                        VALUES ( @room_name, 
                    @room_size, 
                    @monthly_price, 
                    @daily_price, 
                    @hourly_price, 
                    @zone_id, 
                    @building_id, 
                    @floor_id, 
                    @rental_type_id, 
                    @createby, 
                    @createatutc, 
                    @active, 
                    @inactivedate)
                RETURNING id;";

                if (rental_space.active == "Y")
                {
                    rental_space.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    room_name = rental_space.room_name,
                    room_size = rental_space.room_size,
                    monthly_price = rental_space.monthly_price,
                    daily_price = rental_space.daily_price,
                    hourly_price = rental_space.hourly_price,
                    zone_id = rental_space.zone_id,
                    building_id = rental_space.building_id,
                    floor_id = rental_space.floor_id,
                    rental_type_id = rental_space.rental_type_id,
                    active = rental_space.active,
                    inactivedate = rental_space.inactivedate,
                    createby = rental_space.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };
                string sql = DapperHelper.DebugSql(query, paraments);
                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM rental_space WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(RentalSpace rental_space)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE rental_space 
                SET 
                room_name = @room_name, 
                room_size = @room_size, 
                monthly_price = @monthly_price, 
                daily_price = @daily_price, 
                hourly_price = @hourly_price, 
                zone_id = @zone_id, 
                building_id = @building_id, 
                floor_id = @floor_id, 
                rental_type_id = @rental_type_id, 
                updateby = @updateby, 
                updateatutc = @updateatutc, 
                active = @active, 
                inactivedate = @inactivedate, 
                deleted = @deleted
                WHERE id = @id;";
                if (rental_space.active == "Y")
                {
                    rental_space.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    id = rental_space.id,
                    room_name = rental_space.room_name,
                    room_size = rental_space.room_size,
                    monthly_price = rental_space.monthly_price,
                    daily_price = rental_space.daily_price,
                    hourly_price = rental_space.hourly_price,
                    zone_id = rental_space.zone_id,
                    building_id = rental_space.building_id,
                    floor_id = rental_space.floor_id,
                    rental_type_id = rental_space.rental_type_id,
                    deleted = rental_space.deleted,
                    active = rental_space.active,
                    inactivedate = rental_space.inactivedate,
                    updateby = rental_space.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };
                string sql = DapperHelper.DebugSql(query, paraments);
                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<RentalSpace> RentalSpaces, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            select a.*
            ,b.name1 As rental_type_name
            ,c.name1 as floor_name
            ,d.name1 as building_name
            ,e.name1 as zone_name
            from rental_space  a
             inner join rental_type b  on a.rental_type_id =b.id
             inner join floor c on a.floor_id=c.id
             inner join building d on a.building_id=d.id
             inner join zone e on a.zone_id=e.id
            WHERE (@Keyword IS NULL OR 
                   LOWER(room_name) LIKE LOWER(@Keyword) OR
                   LOWER(b.name1) LIKE LOWER(@Keyword) OR
                   LOWER(c.name1) LIKE LOWER(@Keyword) OR
                   LOWER(d.name1) LIKE LOWER(@Keyword) OR
                   LOWER(e.name1) LIKE LOWER(@Keyword))
            ORDER BY id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            from rental_space  a
             inner join rental_type b  on a.rental_type_id =b.id
             inner join floor c on a.floor_id=c.id
             inner join building d on a.building_id=d.id
             inner join zone e on a.zone_id=e.id
            WHERE (@Keyword IS NULL OR 
                   LOWER(room_name) LIKE LOWER(@Keyword) OR
                   LOWER(b.name1) LIKE LOWER(@Keyword) OR
                   LOWER(c.name1) LIKE LOWER(@Keyword) OR
                   LOWER(d.name1) LIKE LOWER(@Keyword) OR
                   LOWER(e.name1) LIKE LOWER(@Keyword));";

                var offset = (pageNumber - 1) * pageSize;

                using (var multi = await connection.QueryMultipleAsync(query, new
                {
                    Keyword = string.IsNullOrWhiteSpace(keyword) ? null : $"%{keyword}%",
                    Offset = offset,
                    PageSize = pageSize
                }))
                {
                    var rental_spaces = await multi.ReadAsync<RentalSpace>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (rental_spaces, totalRecords);
                }
            }
        }

        public async Task<RentalSpace?> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM rental_space
            WHERE id = @id;";

                return await connection.QueryFirstOrDefaultAsync<RentalSpace>(query, new { id = id });
            }
        }

    }
}
