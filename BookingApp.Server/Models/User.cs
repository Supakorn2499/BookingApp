using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApp.Server.Models
{
    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string token { get; set; }
        public string usertype { get; set; }
        public string createby { get; set; }
        public DateTime? createatutc { get; set; }
        public string updateby { get; set; }
        public DateTime? updateatutc { get; set; }
        public string createapp { get; set; }
        public string code { get; set; }
        public string name1 { get; set; }
        public string name2 { get; set; }
        public string tel { get; set; }
        public string mobile { get; set; }
        public string line { get; set; }
        public string active { get; set; }
        public DateTime? inactivedate { get; set; }
        public DateTime? start_work_date { get; set; }
        public string nick_name { get; set; }
        public int roleid { get; set; }
        public string confirmpassword { get; set; }
        public string picture { get; set; }
        public string picture2 { get; set; }
    }
}
