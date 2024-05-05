namespace CarRentalManagement.API.Dtos
{
    public class GetCustomerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LicenseNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } // Assuming 'Role' is a property of 'Customer'
    }
}
