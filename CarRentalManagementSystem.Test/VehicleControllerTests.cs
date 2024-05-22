//using Xunit; // Use xUnit
//using Moq;
//using Microsoft.AspNetCore.Mvc;
//using CarRentalManagement.API.Controllers;
//using CarRentalManagement.Repository.Interfaces;
//using CarRentalManagement.Repository.Models;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace CarRentalManagementSystem.Test // Ensure this matches your folder structure
//{
//    public class VehicleControllerTests
//    {
//        private readonly VehicleController _controller;
//        private readonly Mock<IVehicleRepository> _mockRepo;

//        public VehicleControllerTests()
//        {
//            _mockRepo = new Mock<IVehicleRepository>();
//            _controller = new VehicleController(_mockRepo.Object);
//        }

//        [Fact]
//        public async Task GetVehicles_ReturnsOkResult_WithListOfVehicles()
//        {
//            // Arrange
//            var vehicles = new List<Vehicle>
//            {
//                new Vehicle { Id = 1, Make = "Toyota", Model = "Corolla" },
//                new Vehicle { Id = 2, Make = "Honda", Model = "Civic" }
//            };
//            _mockRepo.Setup(repo => repo.GetAllVehiclesAsync()).ReturnsAsync(vehicles);

//            // Act
//            var result = await _controller.GetVehicles();

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result.Result);
//            var returnVehicles = Assert.IsType<List<Vehicle>>(okResult.Value);
//            Assert.Equal(2, returnVehicles.Count);
//        }

//        [Fact]
//        public async Task GetVehicle_ReturnsOkResult_WithVehicle()
//        {
//            // Arrange
//            var vehicleId = 1;
//            var vehicle = new Vehicle { Id = vehicleId, Make = "Toyota", Model = "Corolla" };
//            _mockRepo.Setup(repo => repo.GetVehicleByIdAsync(vehicleId)).ReturnsAsync(vehicle);

//            // Act
//            var result = await _controller.GetVehicle(vehicleId);

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result.Result);
//            var returnVehicle = Assert.IsType<Vehicle>(okResult.Value);
//            Assert.Equal(vehicleId, returnVehicle.Id);
//            Assert.Equal("Toyota", returnVehicle.Make);
//            Assert.Equal("Corolla", returnVehicle.Model);
//        }

//        [Fact]
//        public async Task GetVehicle_ReturnsNotFound_WhenVehicleNotFound()
//        {
//            // Arrange
//            var vehicleId = 1;
//            _mockRepo.Setup(repo => repo.GetVehicleByIdAsync(vehicleId)).ReturnsAsync((Vehicle)null);

//            // Act
//            var result = await _controller.GetVehicle(vehicleId);

//            // Assert
//            Assert.IsType<NotFoundResult>(result.Result);
//        }
//    }
//}
