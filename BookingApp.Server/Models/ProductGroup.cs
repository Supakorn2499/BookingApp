namespace BookingApp.Server.Models
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
        public int accbuycashid { get; set; }
        public int accbuycreditid { get; set; }
        public int accsalecashid { get; set; }
        public int accsalecreditid { get; set; }
        public int acccostid { get; set; }
        public int accstockid { get; set; }
        public int accworkinprocessid { get; set; }
        public int accsuppliedusedid { get; set; }
        public int accmaterialrequistionid { get; set; }
        public int budgetgroupid { get; set; }
        public int acccngoodid { get; set; }
        public int acccnpriceid { get; set; }
        public int accincreasedebtreturnincreaseid { get; set; }
        public int accincreasedebtpriceincreaseid { get; set; }
        public string createby { get; set; }
        public DateTime? createatutc { get; set; }
        public string updateby { get; set; }
        public DateTime? updateatutc { get; set; }
        public string createapp { get; set; }
    }
}
