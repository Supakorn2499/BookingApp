using BookingApp.Server.Models;
using Dapper;
using Npgsql;
using System.Numerics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BookingApp.Server.Services
{
    public class CompanyRepository
    {
        private readonly string _connectionString;

        public CompanyRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(Company company)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                INSERT INTO company (
                    code, taxid, name1, name2, houseno, moo, floor, room, village, village2, building, building2,
                    lane, lane2, yaek, road, road2, subdisrict_code, disrict_code, province_code, country_code, postal_code,
                    tel2, tel, tel_etax, mobile, email, fax, website, onwername, registerno, createby, createatutc
                )
                VALUES (
                    @code, @taxid, @name1, @name2, @houseno, @moo, @floor, @room, @village, @village2, @building, @building2,
                    @lane, @lane2, @yaek, @road, @road2, @subdisrict_code, @disrict_code, @province_code, @country_code, @postal_code,
                    @tel2, @tel, @tel_etax, @mobile, @email, @fax, @website, @onwername, @registerno, @createby, @createatutc
                ) RETURNING id;";


                var paraments = new
                {
                    code = company.code,
                    taxid = company.taxid,
                    name1 = company.name1,
                    name2 = company.name2,
                    houseno = company.houseno,
                    moo = company.moo,
                    floor = company.floor,
                    room = company.room,
                    village = company.village,
                    village2 = company.village2,
                    building = company.building,
                    building2 = company.building2,
                    lane = company.lane,
                    lane2 = company.lane2,
                    yaek = company.yaek,
                    road = company.road,
                    road2 = company.road2,
                    subdisrict_code = company.subdisrict_code,
                    disrict_code = company.disrict_code,
                    province_code = company.province_code,
                    country_code = company.country_code,
                    postal_code = company.postal_code,
                    tel2 = company.tel2,
                    tel = company.tel,
                    tel_etax = company.tel_etax,
                    mobile = company.mobile,
                    email = company.email,
                    fax = company.fax,
                    website = company.website,
                    onwername = company.onwername,
                    registerno = company.registerno,
                    createby = company.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM company WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(Company company)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE company 
                SET 
                code = @code,
                taxid = @taxid,
                name1 = @name1,
                name2 = @name2,
                houseno = @houseno,
                moo = @moo,
                floor = @floor,
                room = @room,
                village = @village,
                village2 = @village2,
                building = @building,
                building2 = @building2,
                lane = @lane,
                lane2 = @lane2,
                yaek = @yaek,
                road = @road,
                road2 = @road2,
                subdisrict_code = @subdisrict_code,
                disrict_code = @disrict_code,
                province_code = @province_code,
                country_code = @country_code,
                postal_code = @postal_code,
                tel2 = @tel2,
                tel = @tel,
                tel_etax = @tel_etax,
                mobile = @mobile,
                email = @email,
                fax = @fax,
                website = @website,
                onwername = @onwername,
                registerno = @registerno,
                updateby = @updateby,
                updateatutc = @updateatutc
                WHERE id = @id;";

                var paraments = new
                {
                    id = company.id,
                    code = company.code,
                    taxid = company.taxid,
                    name1 = company.name1,
                    name2 = company.name2,
                    houseno = company.houseno,
                    moo = company.moo,
                    floor = company.floor,
                    room = company.room,
                    village = company.village,
                    village2 = company.village2,
                    building = company.building,
                    building2 = company.building2,
                    lane = company.lane,
                    lane2 = company.lane2,
                    yaek = company.yaek,
                    road = company.road,
                    road2 = company.road2,
                    subdisrict_code = company.subdisrict_code,
                    disrict_code = company.disrict_code,
                    province_code = company.province_code,
                    country_code = company.country_code,
                    postal_code = company.postal_code,
                    tel2 = company.tel2,
                    tel = company.tel,
                    tel_etax = company.tel_etax,
                    mobile = company.mobile,
                    email = company.email,
                    fax = company.fax,
                    website = company.website,
                    onwername = company.onwername,
                    registerno = company.registerno,
                    updateby = company.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<Company> Companys, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM company
            WHERE (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(name2) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword))
            ORDER BY id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            FROM company
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
                    var companys = await multi.ReadAsync<Company>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (companys, totalRecords);
                }
            }
        }

        public async Task<Company?> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM company
            WHERE id = @id;";

                return await connection.QueryFirstOrDefaultAsync<Company>(query, new { id = id });
            }
        }

    }
}
