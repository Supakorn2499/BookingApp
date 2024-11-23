using BookingApp.Server.Models;
using Dapper;
using Npgsql;

namespace BookingApp.Server.Services
{
    public class AddressRepository
    {
        private readonly string _connectionString;

        public AddressRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(Address address)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                INSERT INTO address (
                    addresstype, reftype, refid, no, moo,
                    floor, room, village, village2, building,
                    building2, soi, soi2, yaek, road,
                    road2, subdisrict, subdisrict2, disrict, disrict2,
                    province, province2, country, country2, zipcode,
                    createby, createatutc
                )
                VALUES (
                    @addresstype, @reftype, @refid, @no, @moo,
                    @floor, @room, @village, @village2, @building,
                    @building2, @soi, @soi2, @yaek, @road,
                    @road2, @subdisrict, @subdisrict2, @disrict, @disrict2,
                    @province, @province2, @country, @country2, @zipcode,
                    @createby, @createatutc
                )
                RETURNING id;";

                var paraments = new
                {
                    addresstype = address.addresstype,
                    reftype = address.reftype,
                    refid = address.refid,
                    no = address.no,
                    moo = address.moo,
                    floor = address.floor,
                    room = address.room,
                    village = address.village,
                    village2 = address.village2,
                    building = address.building,
                    building2 = address.building2,
                    soi = address.soi,
                    soi2 = address.soi2,
                    yaek = address.yaek,
                    road = address.road,
                    road2 = address.road2,
                    subdisrict = address.subdisrict,
                    subdisrict2 = address.subdisrict2,
                    disrict = address.disrict,
                    disrict2 = address.disrict2,
                    province = address.province,
                    province2 = address.province2,
                    country = address.country,
                    country2 = address.country2,
                    zipcode = address.zipcode,
                    createby = address.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM address WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(Address address)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE address 
                SET 
                    addresstype = @addresstype,
                    reftype = @reftype,
                    refid = @refid,
                    no = @no,
                    moo = @moo,
                    floor = @floor,
                    room = @room,
                    village = @village,
                    village2 = @village2,
                    building = @building,
                    building2 = @building2,
                    soi = @soi,
                    soi2 = @soi2,
                    yaek = @yaek,
                    road = @road,
                    road2 = @road2,
                    subdisrict = @subdisrict,
                    subdisrict2 = @subdisrict2,
                    disrict = @disrict,
                    disrict2 = @disrict2,
                    province = @province,
                    province2 = @province2,
                    country = @country,
                    country2 = @country2,
                    zipcode = @zipcode,
                    updateby=@updateby, 
                    updateatutc=@updateatutc
                WHERE id = @id;";

                var paraments = new
                {
                    id = address.id,
                    addresstype = address.addresstype,
                    reftype = address.reftype,
                    refid = address.refid,
                    no = address.no,
                    moo = address.moo,
                    floor = address.floor,
                    room = address.room,
                    village = address.village,
                    village2 = address.village2,
                    building = address.building,
                    building2 = address.building2,
                    soi = address.soi,
                    soi2 = address.soi2,
                    yaek = address.yaek,
                    road = address.road,
                    road2 = address.road2,
                    subdisrict = address.subdisrict,
                    subdisrict2 = address.subdisrict2,
                    disrict = address.disrict,
                    disrict2 = address.disrict2,
                    province = address.province,
                    province2 = address.province2,
                    country = address.country,
                    country2 = address.country2,
                    zipcode = address.zipcode,
                    updateby = address.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }


        public async Task<Address?> GetByIdAsync(int addresstype, int refid)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM address
            WHERE addresstype = @addresstype and refid = @refid;";

                return await connection.QueryFirstOrDefaultAsync<Address>(query, new { addresstype = addresstype, refid = refid });
            }
        }

    }
}
