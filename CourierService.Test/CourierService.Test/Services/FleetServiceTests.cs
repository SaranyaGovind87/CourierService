using Xunit;
using CourierService.Models;
using CourierService.Services;

namespace CourierService.Tests
{
    public class FleetTests
    {
        [Fact]
        public void CalculateTimes_ShouldMatchRequirementExample()
        {
            // Arrange
            var service = new FleetService();
            var vehicles = new List<Vehicle> { new Vehicle(1, 70, 200), new Vehicle(2, 70, 200) };
            var packages = new List<Package>
            {
                new Package("PKG1", 50, 30, "OFR001"),
                new Package("PKG2", 75, 125, "OFR008"),
                new Package("PKG3", 175, 100, "OFR003"),
                new Package("PKG4", 110, 60, "OFR002"),
                new Package("PKG5", 155, 95, "NA")
            };

            // Act
            service.CalculateDeliveryTimes(packages, vehicles);

            // Assert
            var p4 = packages.First(p => p.Id == "PKG4");
            var p3 = packages.First(p => p.Id == "PKG3");
            var p5 = packages.First(p => p.Id == "PKG5");

            // Using 2 decimal precision as per PDF Page 13
            Assert.Equal(0.85m, Math.Round(p4.EstimatedDeliveryTime, 2));
            Assert.Equal(1.42m, Math.Round(p3.EstimatedDeliveryTime, 2));
            Assert.Equal(4.21m, Math.Round(p5.EstimatedDeliveryTime, 2));
        }

        [Fact]
        public void CalculateDeliveryTimes_WithOverweightPackage_ShouldLogAndExitGracefully()
        {
            // Arrange
            var service = new FleetService();

            // Create a vehicle with 200kg capacity
            var vehicles = new List<Vehicle> { new Vehicle(1, 70, 200) };

            // Create a package that is 300kg (impossible to deliver)
            var packages = new List<Package>
            {
                new Package("PKG_OVERWEIGHT", 300, 50, "NA"),
                new Package("PKG_NORMAL", 50, 20, "NA")
            };

            // Act
            // We wrap this in a check to ensure it doesn't take more than a few seconds 
            // (protecting against infinite loops)
            var task = Task.Run(() => service.CalculateDeliveryTimes(packages, vehicles));
            bool completed = task.Wait(TimeSpan.FromSeconds(5));

            // Assert
            Assert.True(completed, "The service should have exited the loop, but it timed out (potential infinite loop).");

            // Verify that the normal package was not delivered because the 
            // service stops when it hits a deadlock it cannot resolve
            var overweightPkg = packages.First(p => p.Id == "PKG_OVERWEIGHT");
            Assert.Equal(0, overweightPkg.EstimatedDeliveryTime);

            // Verify the log file exists and contains the error
            string logContent = File.ReadAllText("delivery_log.txt");
            //Assert.Contains("too heavy", logContent);
            Assert.Contains("PKG_OVERWEIGHT", logContent);
        }
    }
}
