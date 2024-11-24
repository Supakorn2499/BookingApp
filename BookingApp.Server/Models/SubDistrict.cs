namespace BookingApp.Server.Models
{
    public class SubDistrict
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name1 { get; set; }
        public string name2 { get; set; }
        public string districtcode { get; set; }
        public string provincecode { get; set; }
        public string countrycode { get; set; } = "TH";
        public string postal_code { get; set; }
        public string createby { get; set; }
        public DateTime? createatutc { get; set; }
        public string updateby { get; set; }
        public DateTime? updateatutc { get; set; }

    }
}
