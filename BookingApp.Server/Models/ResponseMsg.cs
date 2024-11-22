namespace BookingApp.Server.Models
{
    public class ResponseMsg
    {
       public string Message { get; set; }
        public object Data { get; set; }
        public int Status { get; set; } = 200;
    }
}
