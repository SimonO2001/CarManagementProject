namespace CarRentalManagement.Repository.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LicenseNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; } // Field to store the hashed password
        public string Role { get; set; } = "Default"; // Default role
    }
}
