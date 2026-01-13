using System;
using System.Collections.Generic;
using System.Text;
using CourierService.Domain;
using CourierService.Models;
using CourierService.Strategies;

namespace CourierService.Services
{
    public class DeliveryCostService
    {
        private readonly IEnumerable<IOfferStrategy> _strategies;

        // Dependency Injection allows us to mock strategies or pass specific ones during tests
        public DeliveryCostService(IEnumerable<IOfferStrategy> strategies)
        {
            _strategies = strategies;
        }

        public void CalculateCost(Package pkg, decimal baseCost)
        {
            decimal deliveryCost = baseCost + (pkg.Weight * Constants.WeightMultiplier) + (pkg.Distance * Constants.DistanceMultiplier);
            var strategy = _strategies.FirstOrDefault(s => s.OfferCode == pkg.OfferCode);

            decimal discount = 0;
            if (strategy != null && strategy.IsApplicable(pkg))
            {
                discount = strategy.CalculateDiscount(deliveryCost);
            }

            pkg.DiscountAmount = discount;
            pkg.TotalCost = deliveryCost - discount;
        }
    }
}