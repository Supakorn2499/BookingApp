namespace BookingApp.Server.Models
{
    public class Address
    {
        public int id { get; set; }
        public int? addresstype { get; set; }
        public int? reftype { get; set; }
        public int? refid { get; set; }
        public string no { get; set; }
        public string moo { get; set; }
        public string floor { get; set; }
        public string room { get; set; }
        public string village { get; set; }
        public string village2 { get; set; }
        public string building { get; set; }
        public string building2 { get; set; }
        public string soi { get; set; }
        public string soi2 { get; set; }
        public string yaek { get; set; }
        public string road { get; set; }
        public string road2 { get; set; }
        public string subdisrict { get; set; }
        public string subdisrict2 { get; set; }
        public string disrict { get; set; }
        public string disrict2 { get; set; }
        public string province { get; set; }
        public string province2 { get; set; }
        public string country { get; set; }
        public string country2 { get; set; }
        public string zipcode { get; set; }
        public string createby { get; set; }
        public DateTime? createatutc { get; set; }
        public string updateby { get; set; }
        public DateTime? updateatutc { get; set; }
        public string createapp { get; set; }
    }

}
