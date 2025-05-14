// Start of file
namespace TestApp.Models
{
    // Start of Farmer class
    public class Farmer
    {
        // Primary key for Farmer
        public int Id { get; set; }

        // Display name of the farmer
        public string Name { get; set; }

        // Contact information (e.g., phone or email)
        public string ContactInfo { get; set; }

        // Physical address of the farmer
        public string Address { get; set; }

        // Foreign key linking to the User record
        public int UserId { get; set; }
    }
    // End of Farmer class
}
// End of file
