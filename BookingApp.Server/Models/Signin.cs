using System.ComponentModel.DataAnnotations;

namespace BookingApp.Server.Models
{
    public class Signin
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }  

        //[Required]
        //public string Role { get; set; } 
    }
}
