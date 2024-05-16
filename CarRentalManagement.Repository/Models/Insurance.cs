namespace CarRentalManagement.Repository.Models
{
    public class Insurance
    {
        public int Id { get; set; }
        public int? VehicleId { get; set; }  // Make VehicleId nullable
        public string Provider { get; set; }
        public string PolicyNumber { get; set; }
        public string Coverage { get; set; }
        public decimal CostADay { get; set; }
        public Vehicle? Vehicle { get; set; }
        // Additional properties...
    }
}
