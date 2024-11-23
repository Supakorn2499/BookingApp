namespace BookingApp.Server.Models
{
    public class Vendor
    {
        public int id { get; set; }
        public int? companyid { get; set; } = 0;
        public int? vendorgroupid { get; set; } = 0;
        public int? acchartid { get; set; } = 0;
        public int? bankid { get; set; } = 0;
        public int? bankbranchid { get; set; } = 0;
        public int? paytypeid { get; set; } = 0;
        public string text_report_vendortype { get; set; }
        public string text_report_taxid { get; set; }
        public string text_report_cid { get; set; }
        public string text_report_prename { get; set; }
        public string text_report_firstname { get; set; }
        public string text_report_midname { get; set; }
        public string text_report_lastname { get; set; }
        public string text_report_name1 { get; set; }
        public string text_report_sname1 { get; set; }
        public string text_report_headoffice { get; set; }
        public string text_report_branchcode { get; set; }
        public string text_report_branchname { get; set; }
        public string code { get; set; }
        public string vendortype { get; set; }
        public string status { get; set; }
        public string active { get; set; }
        public DateTime? inactivedate { get; set; }
        public string prename { get; set; }
        public string firstname { get; set; }
        public string midname { get; set; }
        public string lastname { get; set; }
        public string firstname2 { get; set; }
        public string midname2 { get; set; }
        public string lastname2 { get; set; }
        public string name1 { get; set; }
        public string sname1 { get; set; }
        public string name2 { get; set; }
        public string sname2 { get; set; }
        public string taxid { get; set; }
        public string cid { get; set; }
        public string headoffice { get; set; }
        public string branchcode { get; set; }
        public string branchname { get; set; }
        public string branchname2 { get; set; }
        public string teletax { get; set; }
        public string tel { get; set; }
        public string mobile { get; set; }
        public string fax { get; set; }
        public string website { get; set; }
        public string email { get; set; }
        public string singlelineaddress { get; set; }
        public string singlelineaddress2 { get; set; }
        public string shippingby { get; set; }
        public string bussinesstype { get; set; }
        public decimal? creditdays { get; set; } = 0;
        public decimal? creditamt { get; set; } = 0;
        public string methodofpaymentcode { get; set; }
        public string bookbankno { get; set; }
        public string bookbankname { get; set; }
        public string bankcode { get; set; }
        public string bankbranch { get; set; }
        public string billingcond { get; set; }
        public string creditdaysbilling { get; set; }
        public string receivingdatecond { get; set; }
        public string discountendbill { get; set; }
        public string discountitem { get; set; }
        public string currencycode { get; set; }
        public string promotioncode { get; set; }
        public string vattype { get; set; }
        public string vatisout { get; set; }
        public decimal? vatrate { get; set; }
        public string remark1 { get; set; }
        public string remark2 { get; set; }
        public string remark3 { get; set; }
        public string remark4 { get; set; }
        public string remark5 { get; set; }
        public string remark6 { get; set; }
        public string remark7 { get; set; }
        public string remark8 { get; set; }
        public string remark9 { get; set; }
        public string remark10 { get; set; }
        public string createby { get; set; }
        public DateTime? createatutc { get; set; }
        public string updateby { get; set; }
        public DateTime? updateatutc { get; set; }
        public string createapp { get; set; }
        public string ivattype { get; set; }
        public string lineid { get; set; }
        public int? iwhttype { get; set; } = 0;
        public int? shippingid { get; set; } = 0;
    }
}
