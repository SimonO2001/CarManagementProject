namespace CarRentalManagement.API.Dtos
{
    public class CustomerCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LicenseNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }  // This will be used to capture the password from the request
        public string Role { get; set; } = "Default";
    }
}
