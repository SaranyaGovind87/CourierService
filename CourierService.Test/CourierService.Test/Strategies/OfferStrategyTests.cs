using System;
using System.Collections.Generic;
using System.Text;

using CourierService.Models;
using CourierService.Strategies;
using Xunit;

namespace CourierService.Tests.Strategies
{
    public class OfferStrategyTests
    {
        [Theory]
        // OFR001: Dist < 200, Wgt 70-200. Discount 10%
        [InlineData(100, 100, true)]  // Valid
        [InlineData(200, 100, false)] // Invalid Dist (Must be < 200)
        [InlineData(100, 69, false)]  // Invalid Weight (Low)
        [InlineData(100, 201, false)] // Invalid Weight (High)
        public void Offer001_ValidatesCriteriaCorrectly(int distance, int weight, bool expected)
        {
            var strategy = new Offer001();
            var pkg = new Package("PKG1", weight, distance, "OFR001");

            Assert.Equal(expected, strategy.IsApplicable(pkg));
        }

        [Fact]
        public void Offer001_CalculatesCorrectDiscount()
        {
            var strategy = new Offer001();
            decimal cost = 1000m;
            // 10% of 1000 is 100
            Assert.Equal(100m, strategy.CalculateDiscount(cost));
        }

        [Theory]
        // OFR003: Dist 50-250, Wgt 10-150. Discount 5%
        [InlineData(10, 100, false)] // Invalid Dist (< 50)
        [InlineData(50, 10, true)]   // Valid Boundary
        [InlineData(250, 150, true)] // Valid Boundary
        public void Offer003_ValidatesCriteriaCorrectly(int distance, int weight, bool expected)
        {
            var strategy = new Offer003();
            var pkg = new Package("PKG1", weight, distance, "OFR003");
            Assert.Equal(expected, strategy.IsApplicable(pkg));
        }
    }
}
