namespace CarRentalManagement.API.Dtos
{
    public class RentalContractDto
    {
        public int VehicleId { get; set; }
        public int CustomerId { get; set; }
        public int InsuranceId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalCost { get; set; }
    }
}
