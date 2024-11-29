namespace BookingApp.Server.Models
{
    public class BookBank
    {
        public int id { get; set; }
        public int? companyid { get; set; }
        public int? bankid { get; set; }
        public string bankname {  get; set; }
        public int? bankbranchid { get; set; }        
        public string bankbranchname { get; set; }
        public string code { get; set; }
        public string bookno { get; set; }
        public string name1 { get; set; }
        public string name2 { get; set; }        
        public DateTime? opendate { get; set; }
        public decimal? balanceamount { get; set; }
        public DateTime? blancedate { get; set; }
        public string active { get; set; }
        public DateTime? inactivedate { get; set; }
        public string createby { get; set; }
        public DateTime? createatutc { get; set; }
        public string updateby { get; set; }
        public DateTime? updateatutc { get; set; }
        public string createapp { get; set; }
    }
}
