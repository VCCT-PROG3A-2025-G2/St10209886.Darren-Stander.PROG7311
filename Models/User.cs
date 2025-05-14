// Start of file
namespace TestApp.Models
{
    // Start of User class
    public class User
    {
        // Primary key for the user
        public int Id { get; set; }

        // Username used for login
        public string Username { get; set; }

        // Password for authentication
        public string Password { get; set; }

        // Role assigned to the user (e.g., "Farmer" or "Employee")
        public string Role { get; set; }

        // Optional foreign key linking to a Farmer profile
        public int? FarmerId { get; set; }
    }
    // End of User class
}
// End of file
