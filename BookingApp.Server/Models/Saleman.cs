namespace BookingApp.Server.Models
{
    public class Saleman
    {
        public int id { get; set; }
        public int companyid { get; set; }
        public string code { get; set; }
        public string card_no { get; set; }
        public string prefix_th { get; set; }
        public string prefix_en { get; set; }
        public string name1 { get; set; }
        public string name2 { get; set; }
        public string tel { get; set; }
        public string mobile { get; set; }
        public string line { get; set; }
        public string email { get; set; }
        public string position { get; set; }
        public string login { get; set; }
        public decimal? commission { get; set; }
        public string signature { get; set; }
        public int status { get; set; }
        public string active { get; set; }
        public DateTime? inactivedate { get; set; }
        public string createby { get; set; }
        public DateTime? createatutc { get; set; }
        public string updateby { get; set; }
        public DateTime? updateatutc { get; set; }
        public string createapp { get; set; }
        public int sale_team_id { get; set; }
        public int sale_area_id { get; set; }
        public string nick_name { get; set; }
        public DateTime? start_work_date { get; set; }
    }
}
