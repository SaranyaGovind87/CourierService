using System;
using System.Collections.Generic;
using System.Text;
using CourierService.Models;

namespace CourierService.Strategies
{
    public interface IOfferStrategy
    {
        string OfferCode { get; }
        bool IsApplicable(Package package);
        decimal CalculateDiscount(decimal deliveryCost);
    }
}
