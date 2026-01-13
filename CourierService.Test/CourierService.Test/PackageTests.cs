using Xunit;
using CourierService.Models;

namespace CourierService.Tests
{
    public class PackageTests
    {
        [Theory]
        [InlineData("PKG1", 50, 30, "ofr001", "OFR001")] // Tests casing normalization
        [InlineData("PKG2", 100, 50, "  ", "NA")]       // Tests whitespace handling
        [InlineData("PKG3", 75, 125, null, "NA")]       // Tests null handling
        public void Package_Constructor_ShouldSanitizeOfferCode(string id, int w, int d, string? inputCode, string expectedCode)
        {
            // Act
            var pkg = new Package(id, w, d, inputCode);

            // Assert
            Assert.Equal(expectedCode, pkg.OfferCode);
        }

        [Fact]
        public void Program_Input_ShouldProperlyInitializePackage()
        {
            // Simulate what happens in Program.cs
            var pkg = new Package("PKG_TEST", 100, 50, null);

            Assert.Equal("NA", pkg.OfferCode);
            Assert.Equal(100, pkg.Weight);
        }
    }
}