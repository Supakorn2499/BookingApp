namespace BookingApp.Server.Models
{
    public class Meter
    {
        public int id { get; set; }
        public int meter_type_id { get; set; }
        public string meter_number { get; set; }
        public decimal meter_value { get; set; }
        public int status { get; set; } = 1;
        public string createby { get; set; }
        public DateTime createatutc { get; set; }
        public string updateby { get; set; }
        public DateTime updateatutc { get; set; }
        public string active { get; set; }
        public DateTime? inactivedate { get; set; }
        public int deleted { get; set; } = 0;
        public int rental_space_id { get; set; }

    }
}
