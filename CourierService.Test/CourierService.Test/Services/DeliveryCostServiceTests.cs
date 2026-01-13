using System;
using System.Collections.Generic;
using System.Text;

using CourierService.Models;
using CourierService.Services;
using CourierService.Strategies;
using Xunit;

namespace CourierService.Tests.Services
{
    public class DeliveryCostServiceTests
    {
        private readonly DeliveryCostService _service;

        public DeliveryCostServiceTests()
        {
            // Setup real strategies for integration testing logic
            var strategies = new List<IOfferStrategy>
            {
                new Offer001(), new Offer002(), new Offer003()
            };
            _service = new DeliveryCostService(strategies);
        }

        [Fact]
        public void CalculateCost_WithValidOffer_AppliesDiscount()
        {
            // Arranges
            // Weight 10, Dist 100, Offer OFR003 (Valid: 5% off)
            // Cost = 100 + (10*10) + (100*5) = 100 + 100 + 500 = 700
            // Discount = 5% of 700 = 35
            // Total = 665
            var pkg = new Package("PKG3", 10, 100, "OFR003");
            decimal baseCost = 100;

            // Act
            _service.CalculateCost(pkg, baseCost);

            // Assert
            Assert.Equal(700, pkg.TotalCost + pkg.DiscountAmount); // Verify gross cost
            Assert.Equal(35, pkg.DiscountAmount);
            Assert.Equal(665, pkg.TotalCost);
        }

        [Fact]
        public void CalculateCost_WithInvalidOffer_ZeroDiscount()
        {
            // OFR001 requires Weight > 70. Here Weight is 10.
            var pkg = new Package("PKG1", 10, 100, "OFR001");
            decimal baseCost = 100;

            _service.CalculateCost(pkg, baseCost);

            Assert.Equal(0, pkg.DiscountAmount);
            Assert.Equal(700, pkg.TotalCost);
        }
    }
}
