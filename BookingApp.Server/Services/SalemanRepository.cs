using BookingApp.Server.Models;
using Dapper;
using Npgsql;
using System.Reflection;

namespace BookingApp.Server.Services
{
    public class SalemanRepository
    {
        private readonly string _connectionString;

        public SalemanRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(Saleman saleman)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                INSERT INTO saleman (companyid,code,card_no ,prefix_th ,
                prefix_en ,nick_name , name1,name2, tel,mobile,email,commission,sale_team_id ,
                start_work_date,status,active,inactivedate,createby, createatutc) 
                VALUES (@companyid,@code,@card_no ,@prefix_th ,@prefix_en ,@nick_name , 
                @name1,@name2, @tel,@mobile,@email,@commission,@sale_team_id ,@start_work_date, 
                @status,@active,@inactivedate,@createby, @createatutc)
                RETURNING id;";

                if (saleman.active == "Y")
                {
                    saleman.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                if (saleman.start_work_date != null)
                {
                    saleman.start_work_date = DateTimeHelper.ConvertToUtc(saleman.start_work_date);
                }
                var paraments = new
                {
                    companyid = saleman.companyid,
                    code = saleman.code,
                    nick_name = saleman.nick_name,
                    name1 = saleman.name1,
                    card_no = saleman.card_no,
                    prefix_th = saleman.prefix_th,
                    prefix_en = saleman.prefix_en,
                    name2 = saleman.name2,
                    tel = saleman.tel,
                    mobile = saleman.mobile,
                    email = saleman.email,
                    commission = saleman.commission,
                    sale_team_id = saleman.sale_team_id,
                    status = saleman.status,
                    active = saleman.active,
                    inactivedate = saleman.inactivedate,
                    start_work_date = saleman.start_work_date,
                    createby = saleman.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM saleman WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(Saleman saleman)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE saleman 
                SET 
                    companyid=@companyid,
                    code=@code,
                    card_no=@card_no,
                    prefix_th=@prefix_th,
                    prefix_en=@prefix_en,
                    nick_name=@nick_name ,
                    name1=@name1,
                    name2=@name2,
                    tel=@tel,
                    mobile=@mobile,
                    email=@email,
                    commission=@commission,
                    sale_team_id=@sale_team_id ,
                    start_work_date=@start_work_date ,
                    active=@active,
                    inactivedate=@inactivedate,
                    updateby=@updateby, 
                    updateatutc=@updateatutc
                WHERE id = @id;";
                if (saleman.active == "Y")
                {
                    saleman.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                if (saleman.start_work_date != null)
                {
                    saleman.start_work_date = DateTimeHelper.ConvertToUtc(saleman.start_work_date);
                }
                var paraments = new
                {
                    id = saleman.id,
                    companyid = saleman.companyid,
                    code = saleman.code,
                    nick_name = saleman.nick_name,
                    name1 = saleman.name1,
                    card_no = saleman.card_no,
                    prefix_th = saleman.prefix_th,
                    prefix_en = saleman.prefix_en,
                    name2 = saleman.name2,
                    tel = saleman.tel,
                    mobile = saleman.mobile,
                    email = saleman.email,
                    commission = saleman.commission,
                    sale_team_id = saleman.sale_team_id,
                    active = saleman.active,
                    inactivedate = saleman.inactivedate,
                    start_work_date = saleman.start_work_date,
                    updateby = saleman.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<Saleman> Salemans, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM saleman
            WHERE (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(name2) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword))
            ORDER BY id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            FROM saleman
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
                    var salemans = await multi.ReadAsync<Saleman>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (salemans, totalRecords);
                }
            }
        }

        public async Task<Saleman?> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM saleman
            WHERE id = @id;";

                return await connection.QueryFirstOrDefaultAsync<Saleman>(query, new { id = id });
            }
        }

    }
}
