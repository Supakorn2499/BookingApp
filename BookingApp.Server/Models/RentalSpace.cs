namespace BookingApp.Server.Models
{
    public class RentalSpace
    {
        public int id { get; set; }
        public string room_name { get; set; }
        public decimal room_size { get; set; }
        public decimal monthly_price { get; set; }
        public decimal daily_price { get; set; }
        public decimal hourly_price { get; set; }
        public int zone_id { get; set; }
        public string zone_name { get; set; }
        public int building_id { get; set; }
        public string building_name { get; set; }
        public int floor_id { get; set; }
        public string floor_name { get; set; }
        public int rental_type_id { get; set; }
        public string rental_type_name { get; set; }
        public string createby { get; set; }
        public DateTime? createatutc { get; set; }
        public string updateby { get; set; }
        public DateTime? updateatutc { get; set; }
        public string active { get; set; }
        public DateTime? inactivedate { get; set; }
        public int deleted { get; set; }
    }
}
