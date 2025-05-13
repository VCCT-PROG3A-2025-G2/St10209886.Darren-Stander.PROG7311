namespace TestApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int? FarmerId { get; set; } // New - Links to the Farmer Table
    }
}
