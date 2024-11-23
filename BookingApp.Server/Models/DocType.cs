namespace BookingApp.Server.Models
{
    public class DocType
    {
        public int id { get; set; }
        public string system { get; set; }
        public string menu { get; set; }
        public string type { get; set; }
        public string doc_group { get; set; }
        public string code { get; set; }
        public string name1 { get; set; }
        public string name2 { get; set; }
        public string vat { get; set; }
        public string cash { get; set; }
        public string service { get; set; }
        public string tax_refund { get; set; }
        public string createby { get; set; }
        public DateTime? createatutc { get; set; }
        public string updateby { get; set; }
        public DateTime? updateatutc { get; set; }
        public string createapp { get; set; }
        public string stock { get; set; }
        public int? calflag { get; set; }
        public int? stockseq { get; set; }
    }
}
