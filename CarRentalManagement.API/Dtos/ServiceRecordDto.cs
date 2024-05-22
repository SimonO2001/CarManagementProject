namespace CarRentalManagement.API.Dtos
{
    public class ServiceRecordDto
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime DateOfService { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
    }

}
