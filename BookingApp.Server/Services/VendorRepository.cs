using BookingApp.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;
using System.ComponentModel.Design;
using System;
using Dapper;
using Npgsql.Internal.Postgres;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Numerics;

namespace BookingApp.Server.Services
{
    public class VendorRepository
    {
        private readonly string _connectionString;

        public VendorRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(Vendor vendor)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"INSERT INTO vendor(
                companyid, vendorgroupid, acchartid, bankid, bankbranchid, paytypeid,
                text_report_vendortype, text_report_taxid, text_report_cid,
                text_report_prename, text_report_firstname, text_report_midname, text_report_lastname,
                text_report_name1, text_report_sname1, text_report_headoffice, text_report_branchcode,
                text_report_branchname, code, vendortype, status, active, inactivedate, prename, firstname,
                midname, lastname, firstname2, midname2, lastname2, name1, sname1, name2, sname2,
                taxid, cid, headoffice, branchcode, branchname, branchname2, teletax, tel, mobile, fax,
                website, email, singlelineaddress, singlelineaddress2, shippingby, bussinesstype, creditdays,
                creditamt, methodofpaymentcode, bookbankno, bookbankname, bankcode, bankbranch, billingcond,
                creditdaysbilling, receivingdatecond, discountendbill, discountitem, currencycode, promotioncode,
                vattype, vatisout, vatrate, remark1, remark2, remark3, remark4, remark5, remark6, remark7, remark8,
                remark9, remark10, createby, createatutc, ivattype, lineid,
                iwhttype, shippingid
            ) VALUES(
                @companyid, @vendorgroupid, @acchartid, @bankid, @bankbranchid, @paytypeid,
                @text_report_vendortype, @text_report_taxid, @text_report_cid,
                @text_report_prename, @text_report_firstname, @text_report_midname, @text_report_lastname,
                @text_report_name1, @text_report_sname1, @text_report_headoffice, @text_report_branchcode,
                @text_report_branchname, @code, @vendortype, @status, @active, @inactivedate, @prename, @firstname,
                @midname, @lastname, @firstname2, @midname2, @lastname2, @name1, @sname1, @name2, @sname2,
                @taxid, @cid, @headoffice, @branchcode, @branchname, @branchname2, @teletax, @tel, @mobile, @fax,
                @website, @email, @singlelineaddress, @singlelineaddress2, @shippingby, @bussinesstype, @creditdays,
                @creditamt, @methodofpaymentcode, @bookbankno, @bookbankname, @bankcode, @bankbranch, @billingcond,
                @creditdaysbilling, @receivingdatecond, @discountendbill, @discountitem, @currencycode, @promotioncode,
                @vattype, @vatisout, @vatrate, @remark1, @remark2, @remark3, @remark4, @remark5, @remark6, @remark7, @remark8,
                @remark9, @remark10, @createby, @createatutc, @ivattype, @lineid,
                @iwhttype, @shippingid
            ) RETURNING id;";

                if (vendor.active == "Y")
                {
                    vendor.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    companyid = vendor.companyid,
                    vendorgroupid = vendor.vendorgroupid,
                    acchartid = vendor.acchartid,
                    bankid = vendor.bankid,
                    bankbranchid = vendor.bankbranchid,
                    paytypeid = vendor.paytypeid,
                    text_report_vendortype = vendor.text_report_vendortype,
                    text_report_taxid = vendor.text_report_taxid,
                    text_report_cid = vendor.text_report_cid,
                    text_report_prename = vendor.text_report_prename,
                    text_report_firstname = vendor.text_report_firstname,
                    text_report_midname = vendor.text_report_midname,
                    text_report_lastname = vendor.text_report_lastname,
                    text_report_name1 = vendor.text_report_name1,
                    text_report_sname1 = vendor.text_report_sname1,
                    text_report_headoffice = vendor.text_report_headoffice,
                    text_report_branchcode = vendor.text_report_branchcode,
                    text_report_branchname = vendor.text_report_branchname,
                    code = vendor.code,
                    vendortype = vendor.vendortype,
                    status = vendor.status,
                    active = vendor.active,
                    inactivedate = vendor.inactivedate,
                    prename = vendor.prename,
                    firstname = vendor.firstname,
                    midname = vendor.midname,
                    lastname = vendor.lastname,
                    firstname2 = vendor.firstname2,
                    midname2 = vendor.midname2,
                    lastname2 = vendor.lastname2,
                    name1 = vendor.name1,
                    sname1 = vendor.sname1,
                    name2 = vendor.name2,
                    sname2 = vendor.sname2,
                    taxid = vendor.taxid,
                    cid = vendor.cid,
                    headoffice = vendor.headoffice,
                    branchcode = vendor.branchcode,
                    branchname = vendor.branchname,
                    branchname2 = vendor.branchname2,
                    teletax = vendor.teletax,
                    tel = vendor.tel,
                    mobile = vendor.mobile,
                    fax = vendor.fax,
                    website = vendor.website,
                    email = vendor.email,
                    singlelineaddress = vendor.singlelineaddress,
                    singlelineaddress2 = vendor.singlelineaddress2,
                    shippingby = vendor.shippingby,
                    bussinesstype = vendor.bussinesstype,
                    creditdays = vendor.creditdays,
                    creditamt = vendor.creditamt,
                    methodofpaymentcode = vendor.methodofpaymentcode,
                    bookbankno = vendor.bookbankno,
                    bookbankname = vendor.bookbankname,
                    bankcode = vendor.bankcode,
                    bankbranch = vendor.bankcode,
                    billingcond = vendor.billingcond,
                    creditdaysbilling = vendor.creditdaysbilling,
                    receivingdatecond = vendor.receivingdatecond,
                    discountendbill = vendor.discountendbill,
                    discountitem = vendor.discountitem,
                    currencycode = vendor.currencycode,
                    promotioncode = vendor.promotioncode,
                    vattype = vendor.vattype,
                    vatisout = vendor.vatisout,
                    vatrate = vendor.vatrate,
                    remark1 = vendor.remark1,
                    remark2 = vendor.remark2,
                    remark3 = vendor.remark3,
                    remark4 = vendor.remark4,
                    remark5 = vendor.remark5,
                    remark6 = vendor.remark6,
                    remark7 = vendor.remark7,
                    remark8 = vendor.remark8,
                    remark9 = vendor.remark9,
                    remark10 = vendor.remark10,
                    ivattype = vendor.ivattype,
                    lineid = vendor.lineid,
                    iwhttype = vendor.iwhttype,
                    shippingid = vendor.shippingid,
                    createby = vendor.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM vendor WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(Vendor vendor)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE vendor 
                SET 
                companyid = @companyid, 
                vendorgroupid = @vendorgroupid, 
                acchartid = @acchartid, 
                bankid = @bankid, 
                bankbranchid = @bankbranchid, 
                paytypeid = @paytypeid, 
                text_report_vendortype = @text_report_vendortype, 
                text_report_taxid = @text_report_taxid, 
                text_report_cid = @text_report_cid, 
                text_report_prename = @text_report_prename, 
                text_report_firstname = @text_report_firstname, 
                text_report_midname = @text_report_midname, 
                text_report_lastname = @text_report_lastname, 
                text_report_name1 = @text_report_name1, 
                text_report_sname1 = @text_report_sname1, 
                text_report_headoffice = @text_report_headoffice, 
                text_report_branchcode = @text_report_branchcode, 
                text_report_branchname = @text_report_branchname, 
                code = @code,
                vendortype = @vendortype, 
                status = @status, 
                active = @active, 
                inactivedate = @inactivedate,
                prename = @prename, 
                firstname = @firstname, 
                midname = @midname, 
                lastname = @lastname,
                firstname2 = @firstname2,
                midname2 = @midname2,
                lastname2 = @lastname2, 
                name1 = @name1,
                sname1 = @sname1, 
                name2 = @name2, 
                sname2 = @sname2,
                taxid = @taxid,
                cid = @cid, 
                headoffice = @headoffice,
                branchcode = @branchcode,
                branchname = @branchname,
                branchname2 = @branchname2, 
                teletax = @teletax, 
                tel = @tel,
                mobile = @mobile,
                fax = @fax,
                website = @website,
                email = @email, 
                singlelineaddress = @singlelineaddress,
                singlelineaddress2 = @singlelineaddress2, 
                shippingby = @shippingby,
                bussinesstype = @bussinesstype, 
                creditdays = @creditdays, 
                creditamt = @creditamt, 
                methodofpaymentcode = @methodofpaymentcode, 
                bookbankno = @bookbankno,
                bookbankname = @bookbankname, 
                bankcode = @bankcode,
                bankbranch = @bankbranch, 
                billingcond = @billingcond, 
                creditdaysbilling = @creditdaysbilling, 
                receivingdatecond = @receivingdatecond, 
                discountendbill = @discountendbill,
                discountitem = @discountitem, 
                currencycode = @currencycode, 
                promotioncode = @promotioncode,
                vattype = @vattype,
                vatisout = @vatisout, 
                vatrate = @vatrate,
                remark1 = @remark1,
                remark2 = @remark2, 
                remark3 = @remark3,
                remark4 = @remark4, 
                remark5 = @remark5, 
                remark6 = @remark6,
                remark7 = @remark7,
                remark8 = @remark8, 
                remark9 = @remark9, 
                remark10 = @remark10,
                updateby = @updateby, 
                updateatutc = @updateatutc,
                createapp = @createapp,
                ivattype = @ivattype, 
                lineid = @lineid, 
                iwhttype = @iwhttype, 
                shippingid = @shippingid 
                WHERE id = @id;";
                if (vendor.active == "Y")
                {
                    vendor.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    id = vendor.id,
                    companyid = vendor.companyid,
                    vendorgroupid = vendor.vendorgroupid,
                    acchartid = vendor.acchartid,
                    bankid = vendor.bankid,
                    bankbranchid = vendor.bankbranchid,
                    paytypeid = vendor.paytypeid,
                    text_report_vendortype = vendor.text_report_vendortype,
                    text_report_taxid = vendor.text_report_taxid,
                    text_report_cid = vendor.text_report_cid,
                    text_report_prename = vendor.text_report_prename,
                    text_report_firstname = vendor.text_report_firstname,
                    text_report_midname = vendor.text_report_midname,
                    text_report_lastname = vendor.text_report_lastname,
                    text_report_name1 = vendor.text_report_name1,
                    text_report_sname1 = vendor.text_report_sname1,
                    text_report_headoffice = vendor.text_report_headoffice,
                    text_report_branchcode = vendor.text_report_branchcode,
                    text_report_branchname = vendor.text_report_branchname,
                    code = vendor.code,
                    vendortype = vendor.vendortype,
                    status = vendor.status,
                    active = vendor.active,
                    inactivedate = vendor.inactivedate,
                    prename = vendor.prename,
                    firstname = vendor.firstname,
                    midname = vendor.midname,
                    lastname = vendor.lastname,
                    firstname2 = vendor.firstname2,
                    midname2 = vendor.midname2,
                    lastname2 = vendor.lastname2,
                    name1 = vendor.name1,
                    sname1 = vendor.sname1,
                    name2 = vendor.name2,
                    sname2 = vendor.sname2,
                    taxid = vendor.taxid,
                    cid = vendor.cid,
                    headoffice = vendor.headoffice,
                    branchcode = vendor.branchcode,
                    branchname = vendor.branchname,
                    branchname2 = vendor.branchname2,
                    teletax = vendor.teletax,
                    tel = vendor.tel,
                    mobile = vendor.mobile,
                    fax = vendor.fax,
                    website = vendor.website,
                    email = vendor.email,
                    singlelineaddress = vendor.singlelineaddress,
                    singlelineaddress2 = vendor.singlelineaddress2,
                    shippingby = vendor.shippingby,
                    bussinesstype = vendor.bussinesstype,
                    creditdays = vendor.creditdays,
                    creditamt = vendor.creditamt,
                    methodofpaymentcode = vendor.methodofpaymentcode,
                    bookbankno = vendor.bookbankno,
                    bookbankname = vendor.bookbankname,
                    bankcode = vendor.bankcode,
                    bankbranch = vendor.bankcode,
                    billingcond = vendor.billingcond,
                    creditdaysbilling = vendor.creditdaysbilling,
                    receivingdatecond = vendor.receivingdatecond,
                    discountendbill = vendor.discountendbill,
                    discountitem = vendor.discountitem,
                    currencycode = vendor.currencycode,
                    promotioncode = vendor.promotioncode,
                    vattype = vendor.vattype,
                    vatisout = vendor.vatisout,
                    vatrate = vendor.vatrate,
                    remark1 = vendor.remark1,
                    remark2 = vendor.remark2,
                    remark3 = vendor.remark3,
                    remark4 = vendor.remark4,
                    remark5 = vendor.remark5,
                    remark6 = vendor.remark6,
                    remark7 = vendor.remark7,
                    remark8 = vendor.remark8,
                    remark9 = vendor.remark9,
                    remark10 = vendor.remark10,
                    ivattype = vendor.ivattype,
                    lineid = vendor.lineid,
                    iwhttype = vendor.iwhttype,
                    shippingid = vendor.shippingid,
                    updateby = vendor.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<Vendor> Vendors, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM vendor
            WHERE (@Keyword IS NULL OR 
                   LOWER(code) LIKE LOWER(@Keyword) OR
                   LOWER(name1) LIKE LOWER(@Keyword) OR
                   LOWER(name2) LIKE LOWER(@Keyword) OR
                   LOWER(text_report_taxid) LIKE LOWER(@Keyword) OR 
                   LOWER(text_report_cid) LIKE LOWER(@Keyword) OR 
                   LOWER(text_report_prename) LIKE LOWER(@Keyword) OR
                   LOWER(text_report_firstname) LIKE LOWER(@Keyword) OR 
                   LOWER(text_report_lastname) LIKE LOWER(@Keyword) OR 
                   LOWER(text_report_name1) LIKE LOWER(@Keyword) OR                  
                   LOWER(taxid) LIKE LOWER(@Keyword))
            ORDER BY id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            FROM vendor
            WHERE (@Keyword IS NULL OR 
                   LOWER(code) LIKE LOWER(@Keyword) OR
                   LOWER(name1) LIKE LOWER(@Keyword) OR
                   LOWER(name2) LIKE LOWER(@Keyword) OR
                   LOWER(text_report_taxid) LIKE LOWER(@Keyword) OR 
                   LOWER(text_report_cid) LIKE LOWER(@Keyword) OR 
                   LOWER(text_report_prename) LIKE LOWER(@Keyword) OR
                   LOWER(text_report_firstname) LIKE LOWER(@Keyword) OR 
                   LOWER(text_report_lastname) LIKE LOWER(@Keyword) OR 
                   LOWER(text_report_name1) LIKE LOWER(@Keyword) OR                  
                   LOWER(taxid) LIKE LOWER(@Keyword));";

                var offset = (pageNumber - 1) * pageSize;

                using (var multi = await connection.QueryMultipleAsync(query, new
                {
                    Keyword = string.IsNullOrWhiteSpace(keyword) ? null : $"%{keyword}%",
                    Offset = offset,
                    PageSize = pageSize
                }))
                {
                    var vendors = await multi.ReadAsync<Vendor>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (vendors, totalRecords);
                }
            }
        }

        public async Task<Vendor?> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM vendor
            WHERE id = @id;";

                return await connection.QueryFirstOrDefaultAsync<Vendor>(query, new { id = id });
            }
        }

    }
}
