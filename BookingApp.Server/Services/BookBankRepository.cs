﻿using BookingApp.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;
using System.ComponentModel.Design;
using System;
using Dapper;

namespace BookingApp.Server.Services
{
    public class BookBankRepository
    {
        private readonly string _connectionString;

        public BookBankRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(BookBank bank)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                INSERT INTO bookbank (
                    companyid, bankid, bankbranchid, acchartid, recivecqacchartid, 
                    paymentcqacchartid, depositbookid, revertcqacchartid, trandferacchartid, code, 
                    bookno, name1, name2, sname1, sname2, 
                    bookbanktype, opendate, balanceamount, blancedate, active, 
                    inactivedate, createby, createatutc
                )
                VALUES (
                    @companyid, @bankid, @bankbranchid, @acchartid, @recivecqacchartid, 
                    @paymentcqacchartid, @depositbookid, @revertcqacchartid, @trandferacchartid, @code, 
                    @bookno, @name1, @name2, @sname1, @sname2, 
                    @bookbanktype, @opendate, @balanceamount, @blancedate, @active, 
                    @inactivedate, @createby, @createatutc
                )
                RETURNING id;";

                if (bank.active == "Y")
                {
                    bank.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    companyid = bank.companyid,
                    bankid = bank.bankid,
                    bankbranchid = bank.bankbranchid,
                    acchartid = bank.acchartid,
                    recivecqacchartid = bank.recivecqacchartid,
                    paymentcqacchartid = bank.paymentcqacchartid,
                    depositbookid = bank.depositbookid,
                    revertcqacchartid = bank.revertcqacchartid,
                    trandferacchartid = bank.trandferacchartid,
                    code = bank.code,
                    bookno = bank.bookno,
                    name1 = bank.name1,
                    name2 = bank.name2,
                    sname1 = bank.sname1,
                    sname2 = bank.sname2,
                    bookbanktype = bank.bookbanktype,
                    opendate = bank.opendate,
                    balanceamount = bank.balanceamount,
                    blancedate = bank.blancedate,
                    active = bank.active,
                    inactivedate = bank.inactivedate,
                    createby = bank.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM bookbank WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(BookBank bank)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE bookbank 
                SET 
                    companyid = @companyid,
                    bankid = @bankid,
                    bankbranchid = @bankbranchid,
                    acchartid = @acchartid,
                    recivecqacchartid = @recivecqacchartid,
                    paymentcqacchartid = @paymentcqacchartid,
                    depositbookid = @depositbookid,
                    revertcqacchartid = @revertcqacchartid,
                    trandferacchartid = @trandferacchartid,
                    code = @code,
                    bookno = @bookno,
                    name1 = @name1,
                    name2 = @name2,
                    sname1 = @sname1,
                    sname2 = @sname2,
                    bookbanktype = @bookbanktype,
                    opendate = @opendate,
                    balanceamount = @balanceamount,
                    blancedate = @blancedate,
                    active = @active,
                    inactivedate = @inactivedate,
                    updateby=@updateby, 
                    updateatutc=@updateatutc
                WHERE id = @id;";
                if (bank.active == "Y")
                {
                    bank.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    id = bank.id,
                    companyid = bank.companyid,
                    bankid = bank.bankid,
                    bankbranchid = bank.bankbranchid,
                    acchartid = bank.acchartid,
                    recivecqacchartid = bank.recivecqacchartid,
                    paymentcqacchartid = bank.paymentcqacchartid,
                    depositbookid = bank.depositbookid,
                    revertcqacchartid = bank.revertcqacchartid,
                    trandferacchartid = bank.trandferacchartid,
                    code = bank.code,
                    bookno = bank.bookno,
                    name1 = bank.name1,
                    name2 = bank.name2,
                    sname1 = bank.sname1,
                    sname2 = bank.sname2,
                    bookbanktype = bank.bookbanktype,
                    opendate = bank.opendate,
                    balanceamount = bank.balanceamount,
                    blancedate = bank.blancedate,
                    active = bank.active,
                    inactivedate = bank.inactivedate,
                    updateby = bank.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };
                //string sql = DapperHelper.DebugSql(query, paraments);
                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<BookBank> BookBanks, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM bookbank
            WHERE (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(name2) LIKE LOWER(@Keyword) OR 
                   LOWER(bookno) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword))
            ORDER BY id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            FROM bookbank
            WHERE (@Keyword IS NULL OR 
                   LOWER(name1) LIKE LOWER(@Keyword) OR 
                   LOWER(name2) LIKE LOWER(@Keyword) OR 
                   LOWER(bookno) LIKE LOWER(@Keyword) OR 
                   LOWER(code) LIKE LOWER(@Keyword));";

                var offset = (pageNumber - 1) * pageSize;

                using (var multi = await connection.QueryMultipleAsync(query, new
                {
                    Keyword = string.IsNullOrWhiteSpace(keyword) ? null : $"%{keyword}%",
                    Offset = offset,
                    PageSize = pageSize
                }))
                {
                    var bookbanks = await multi.ReadAsync<BookBank>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (bookbanks, totalRecords);
                }
            }
        }

        public async Task<BookBank?> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM bookbank
            WHERE id = @id;";

                return await connection.QueryFirstOrDefaultAsync<BookBank>(query, new { id = id });
            }
        }

    }
}