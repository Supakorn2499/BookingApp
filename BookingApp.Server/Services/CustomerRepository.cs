using BookingApp.Server.Models;
using Dapper;
using Npgsql;
using System.ComponentModel.Design;

namespace BookingApp.Server.Services
{
    public class CustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(Customer customer)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"INSERT INTO customer(
                    companyid, custgroupid, membergroupid, salemanid, saleteamid, saleareaid, segmentationid,
                    bankid, receivableacchartid, memberpriceid, bankbranchid, mopid, paytypeid, 
                    text_report_custtype, text_report_taxid, text_report_cid, text_report_prename,
                    text_report_firstname, text_report_midname, text_report_lastname, text_report_name1,
                    text_report_sname1, text_report_headoffice, text_report_branchcode, text_report_branchname,
                    code, custtype, status, active, inactivedate, prename, firstname, midname, lastname,
                    firstname2, midname2, lastname2, name1, sname1, name2, sname2, taxid, cid, headoffice, branchcode,
                    branchname, branchname2, teletax, tel, mobile, fax, website, email, singlelineaddress, 
                    singlelineaddress2, shippingby, bussinesstype, creditdays, creditamt, bookbankno, bookbankname,
                    bankbranch, billingcond, creditdaysbilling, receivingdatecond, discountendbill, discountitem, 
                    currencyid, promotioncode, vattype, vatisout, vatrate, remark1, remark2, remark3, remark4,
                    remark5, remark6, remark7, remark8, remark9, remark10, createby, createatutc, lineid, shippingid
                )
                VALUES (
                    @companyid, @custgroupid, @membergroupid, @salemanid, @saleteamid, @saleareaid, @segmentationid,
                    @bankid, @receivableacchartid, @memberpriceid, @bankbranchid, @mopid, @paytypeid, 
                    @text_report_custtype, @text_report_taxid, @text_report_cid, @text_report_prename,
                    @text_report_firstname, @text_report_midname, @text_report_lastname, @text_report_name1,
                    @text_report_sname1, @text_report_headoffice, @text_report_branchcode, @text_report_branchname,
                    @code, @custtype, @status, @active, @inactivedate, @prename, @firstname, @midname, @lastname,
                    @firstname2, @midname2, @lastname2, @name1, @sname1, @name2, @sname2, @taxid, @cid, @headoffice, @branchcode,
                    @branchname, @branchname2, @teletax, @tel, @mobile, @fax, @website, @email, @singlelineaddress, 
                    @singlelineaddress2, @shippingby, @bussinesstype, @creditdays, @creditamt, @bookbankno, @bookbankname,
                    @bankbranch, @billingcond, @creditdaysbilling, @receivingdatecond, @discountendbill, @discountitem, 
                    @currencyid, @promotioncode, @vattype, @vatisout, @vatrate, @remark1, @remark2, @remark3, @remark4,
                    @remark5, @remark6, @remark7, @remark8, @remark9, @remark10, @createby, @createatutc, @lineid, @shippingid
                ) RETURNING id;";

                if (customer.active == "Y")
                {
                    customer.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    companyid = customer.companyid,
                    custgroupid = customer.custgroupid,
                    membergroupid = customer.membergroupid,
                    salemanid = customer.salemanid,
                    saleteamid = customer.saleteamid,
                    saleareaid = customer.saleareaid,
                    segmentationid = customer.segmentationid,
                    bankid = customer.bankid,
                    receivableacchartid = customer.receivableacchartid,
                    memberpriceid = customer.memberpriceid,
                    bankbranchid = customer.bankbranchid,
                    mopid = customer.mopid,
                    paytypeid = customer.paytypeid,
                    text_report_custtype = customer.text_report_custtype,
                    text_report_taxid = customer.text_report_taxid,
                    text_report_cid = customer.text_report_cid,
                    text_report_prename = customer.text_report_prename,
                    text_report_firstname = customer.text_report_firstname,
                    text_report_midname = customer.text_report_midname,
                    text_report_lastname = customer.text_report_lastname,
                    text_report_name1 = customer.text_report_name1,
                    text_report_sname1 = customer.text_report_sname1,
                    text_report_headoffice = customer.text_report_headoffice,
                    text_report_branchcode = customer.text_report_branchcode,
                    text_report_branchname = customer.text_report_branchname,
                    code = customer.code,
                    custtype = customer.custtype,
                    status = customer.status,
                    active = customer.active,
                    inactivedate = customer.inactivedate,
                    prename = customer.prename,
                    firstname = customer.firstname,
                    midname = customer.midname,
                    lastname = customer.lastname,
                    firstname2 = customer.firstname2,
                    midname2 = customer.midname2,
                    lastname2 = customer.lastname2,
                    name1 = customer.name1,
                    sname1 = customer.sname1,
                    name2 = customer.name2,
                    sname2 = customer.sname2,
                    taxid = customer.taxid,
                    cid = customer.cid,
                    headoffice = customer.headoffice,
                    branchcode = customer.branchcode,
                    branchname = customer.branchname,
                    branchname2 = customer.branchname2,
                    teletax = customer.teletax,
                    tel = customer.tel,
                    mobile = customer.mobile,
                    fax = customer.fax,
                    website = customer.website,
                    email = customer.email,
                    singlelineaddress = customer.singlelineaddress,
                    singlelineaddress2 = customer.singlelineaddress2,
                    shippingby = customer.shippingby,
                    bussinesstype = customer.bussinesstype,
                    creditdays = customer.creditdays,
                    creditamt = customer.creditamt,
                    bookbankno = customer.bookbankno,
                    bookbankname = customer.bookbankname,
                    bankbranch = customer.bankbranch,
                    billingcond = customer.billingcond,
                    creditdaysbilling = customer.creditdaysbilling,
                    receivingdatecond = customer.receivingdatecond,
                    discountendbill = customer.discountendbill,
                    discountitem = customer.discountitem,
                    currencyid = customer.currencyid,
                    promotioncode = customer.promotioncode,
                    vattype = customer.vattype,
                    vatisout = customer.vatisout,
                    vatrate = customer.vatrate,
                    remark1 = customer.remark1,
                    remark2 = customer.remark2,
                    remark3 = customer.remark3,
                    remark4 = customer.remark4,
                    remark5 = customer.remark5,
                    remark6 = customer.remark6,
                    remark7 = customer.remark7,
                    remark8 = customer.remark8,
                    remark9 = customer.remark9,
                    remark10 = customer.remark10,
                    lineid = customer.lineid,
                    shippingid = customer.shippingid,
                    createby = customer.createby,
                    createatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                return await connection.ExecuteScalarAsync<int>(query, paraments);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = "DELETE FROM customer WHERE id = @id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(Customer customer)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
                UPDATE customer 
                SET 
                    companyid = @companyid,
                    custgroupid = @custgroupid,
                    membergroupid = @membergroupid,
                    salemanid = @salemanid,
                    saleteamid = @saleteamid,
                    saleareaid = @saleareaid,
                    segmentationid = @segmentationid,
                    bankid = @bankid,
                    receivableacchartid = @receivableacchartid,
                    memberpriceid = @memberpriceid,
                    bankbranchid = @bankbranchid,
                    mopid = @mopid,
                    paytypeid = @paytypeid,
                    text_report_custtype = @text_report_custtype,
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
                    custtype = @custtype,
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
                    bookbankno = @bookbankno,
                    bookbankname = @bookbankname,
                    bankbranch = @bankbranch,
                    billingcond = @billingcond,
                    creditdaysbilling = @creditdaysbilling,
                    receivingdatecond = @receivingdatecond,
                    discountendbill = @discountendbill,
                    discountitem = @discountitem,
                    currencyid = @currencyid,
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
                    lineid = @lineid,
                    shippingid = @shippingid
                WHERE id = @id;";
                if (customer.active == "Y")
                {
                    customer.inactivedate = DateTimeHelper.ConvertToUtc(DateTime.Now);
                }
                var paraments = new
                {
                    id = customer.id,
                    companyid = customer.companyid,
                    custgroupid = customer.custgroupid,
                    membergroupid = customer.membergroupid,
                    salemanid = customer.salemanid,
                    saleteamid = customer.saleteamid,
                    saleareaid = customer.saleareaid,
                    segmentationid = customer.segmentationid,
                    bankid = customer.bankid,
                    receivableacchartid = customer.receivableacchartid,
                    memberpriceid = customer.memberpriceid,
                    bankbranchid = customer.bankbranchid,
                    mopid = customer.mopid,
                    paytypeid = customer.paytypeid,
                    text_report_custtype = customer.text_report_custtype,
                    text_report_taxid = customer.text_report_taxid,
                    text_report_cid = customer.text_report_cid,
                    text_report_prename = customer.text_report_prename,
                    text_report_firstname = customer.text_report_firstname,
                    text_report_midname = customer.text_report_midname,
                    text_report_lastname = customer.text_report_lastname,
                    text_report_name1 = customer.text_report_name1,
                    text_report_sname1 = customer.text_report_sname1,
                    text_report_headoffice = customer.text_report_headoffice,
                    text_report_branchcode = customer.text_report_branchcode,
                    text_report_branchname = customer.text_report_branchname,
                    code = customer.code,
                    custtype = customer.custtype,
                    status = customer.status,
                    active = customer.active,
                    inactivedate = customer.inactivedate,
                    prename = customer.prename,
                    firstname = customer.firstname,
                    midname = customer.midname,
                    lastname = customer.lastname,
                    firstname2 = customer.firstname2,
                    midname2 = customer.midname2,
                    lastname2 = customer.lastname2,
                    name1 = customer.name1,
                    sname1 = customer.sname1,
                    name2 = customer.name2,
                    sname2 = customer.sname2,
                    taxid = customer.taxid,
                    cid = customer.cid,
                    headoffice = customer.headoffice,
                    branchcode = customer.branchcode,
                    branchname = customer.branchname,
                    branchname2 = customer.branchname2,
                    teletax = customer.teletax,
                    tel = customer.tel,
                    mobile = customer.mobile,
                    fax = customer.fax,
                    website = customer.website,
                    email = customer.email,
                    singlelineaddress = customer.singlelineaddress,
                    singlelineaddress2 = customer.singlelineaddress2,
                    shippingby = customer.shippingby,
                    bussinesstype = customer.bussinesstype,
                    creditdays = customer.creditdays,
                    creditamt = customer.creditamt,
                    bookbankno = customer.bookbankno,
                    bookbankname = customer.bookbankname,
                    bankbranch = customer.bankbranch,
                    billingcond = customer.billingcond,
                    creditdaysbilling = customer.creditdaysbilling,
                    receivingdatecond = customer.receivingdatecond,
                    discountendbill = customer.discountendbill,
                    discountitem = customer.discountitem,
                    currencyid = customer.currencyid,
                    promotioncode = customer.promotioncode,
                    vattype = customer.vattype,
                    vatisout = customer.vatisout,
                    vatrate = customer.vatrate,
                    remark1 = customer.remark1,
                    remark2 = customer.remark2,
                    remark3 = customer.remark3,
                    remark4 = customer.remark4,
                    remark5 = customer.remark5,
                    remark6 = customer.remark6,
                    remark7 = customer.remark7,
                    remark8 = customer.remark8,
                    remark9 = customer.remark9,
                    remark10 = customer.remark10,
                    lineid = customer.lineid,
                    shippingid = customer.shippingid,
                    updateby = customer.updateby,
                    updateatutc = DateTimeHelper.ConvertToUtc(DateTime.Now)
                };

                //string sql = DapperHelper.DebugSql(query, paraments);
                var rowsAffected = await connection.ExecuteAsync(query, paraments);
                return rowsAffected > 0;
            }
        }

        public async Task<(IEnumerable<Customer> customers, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM customer
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
            FROM customer
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
                    var customers = await multi.ReadAsync<Customer>();
                    var totalRecords = await multi.ReadFirstAsync<int>();
                    return (customers, totalRecords);
                }
            }
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string query = @"
            SELECT * 
            FROM customer
            WHERE id = @id;";

                return await connection.QueryFirstOrDefaultAsync<Customer>(query, new { id = id });
            }
        }

    }
}
