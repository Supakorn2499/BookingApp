using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BookingApp.Server.Models;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;

namespace BookingApp.Server.Services
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddUserAsync(User user)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                INSERT INTO users (username,password, firstname,lastname, email,code, name1, mobile,createby, createatutc) 
                VALUES (@username,@password, @firstname,@lastname, @email,@code, @name1, @mobile,@createby, @createatutc)
                RETURNING id;";

                var paraments = new
                {
                    username = user.username,
                    password = HasPassword.Sha1(user.password),
                    firstname = user.firstname,
                    lastname = user.lastname,
                    email = user.email,
                    code = user.code,
                    name1 = user.firstname + " " + user.lastname,
                    mobile = user.mobile,
                    createby = user.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM users WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE users 
                SET 
                    username=@username,
                    firstname=@firstname,
                    lastname=@lastname,
                    email=@email,
                    code=@code, 
                    name1=@name1, 
                    mobile=@mobile,
                    updateby=@updateby, 
                    updateatutc=@updateatutc
                WHERE id = @id;";

                var paraments = new
                {
                    username = user.username,
                    firstname = user.firstname,
                    lastname = user.lastname,
                    email = user.email,
                    code = user.code,
                    name1 = user.firstname + " " + user.lastname,
                    mobile = user.mobile,
                    updateby = user.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<User> Users, int TotalRecords)> GetUsersAsync(string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM users
            WHERE (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword))
            ORDER BY id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            FROM users
            WHERE (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword));";

                var offset = (pageNumber - 1) * pageSize;

                using (var multi = await connection.QueryMultipleAsync(query, new
                {
                    Keyword = string.IsNullOrWhiteSpace(keyword) ? null : $"%{keyword}%",
                    Offset = offset,
                    PageSize = pageSize
                }))
                {
                    var users = await multi.ReadAsync<User>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (users, totalRecords);
                }
            }
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM users
            WHERE id = @id;";

                return await connection.QueryFirstOrDefaultAsync<User>(query, new { id = id });
            }
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM users
            WHERE username = @username;";

                return await connection.QueryFirstOrDefaultAsync<User>(query, new { username = username });
            }
        }

        public async Task<bool> ChangePasswordAsync(User user)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE user 
                SET 
                    username=@username,
                    password =@password,
                    updateby=@updateby, 
                    updateatutc=@updateatutc
                WHERE id = @id;";

                var paraments = new
                {
                    username = user.username,
                    password = HasPassword.Sha1(user.password),
                    updateby = user.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM user
            WHERE username = @id and password= @password";

                return await connection.QueryFirstOrDefaultAsync<User>(query, new { username = username, password = password });
            }
        }


    }
}
