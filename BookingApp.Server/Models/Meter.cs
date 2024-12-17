namespace BookingApp.Server.Models
{
    public class Meter
    {
        public int Id { get; set; }
        public int MeterTypeId { get; set; }
        public string MeterNumber { get; set; }
        public decimal MeterValue { get; set; }
        public bool Status { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateAtUtc { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateAtUtc { get; set; }
        public bool Active { get; set; }
        public DateTime? InactiveDate { get; set; }
        public bool Deleted { get; set; }
        public int RentalSpaceId { get; set; }
    }
}
