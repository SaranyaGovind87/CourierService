using System;
using System.Collections.Generic;
using System.Text;

using CourierService.Models;

namespace CourierService.Strategies
{
    // OFR001: 10% off for Distance < 200, Weight 70-200
    public class Offer001 : IOfferStrategy
    {
        public string OfferCode => "OFR001";
        public bool IsApplicable(Package p) => p.Distance < 200 && p.Weight >= 70 && p.Weight <= 200;
        public decimal CalculateDiscount(decimal cost) => cost * 0.10m;
    }

    // OFR002: 7% off for Distance 50-150, Weight 100-250
    public class Offer002 : IOfferStrategy
    {
        public string OfferCode => "OFR002";
        public bool IsApplicable(Package p) => p.Distance >= 50 && p.Distance <= 150 && p.Weight >= 100 && p.Weight <= 250;
        public decimal CalculateDiscount(decimal cost) => cost * 0.07m;
    }

    // OFR003: 5% off for Distance 50-250, Weight 10-150
    public class Offer003 : IOfferStrategy
    {
        public string OfferCode => "OFR003";
        public bool IsApplicable(Package p) => p.Distance >= 50 && p.Distance <= 250 && p.Weight >= 10 && p.Weight <= 150;
        public decimal CalculateDiscount(decimal cost) => cost * 0.05m;
    }
}
