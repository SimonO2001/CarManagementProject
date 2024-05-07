namespace CarRentalManagement.API.Dtos
{
    public class CustomerUpdateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LicenseNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }  // Optional, used only if updating the password
        public string Role { get; set; }
    }


}
