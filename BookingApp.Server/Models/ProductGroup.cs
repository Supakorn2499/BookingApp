﻿namespace BookingApp.Server.Models
{
    public class ProductGroup
    {
        public int id { get; set; }
        public int companyid { get; set; }
        public string code { get; set; }
        public string name1 { get; set; }
        public string name2 { get; set; }
        public string active { get; set; }
        public DateTime? inactivedate { get; set; }        
        public string createby { get; set; }
        public DateTime? createatutc { get; set; }
        public string updateby { get; set; }
        public DateTime? updateatutc { get; set; }
        public string createapp { get; set; }
    }
}
