using CourierService.Logging;
using CourierService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourierService.Services
{
    public class FleetService
    {
        public void CalculateDeliveryTimes(List<Package> packages, List<Vehicle> vehicles)
        {
            var remainingPackages = new List<Package>(packages);

            while (remainingPackages.Any())
            {
                // 1. Pick vehicle available the soonest
                var currentVehicle = vehicles.OrderBy(v => v.AvailableTime).First();

                // 2. Find best shipment
                var shipment = GetBestShipment(remainingPackages, currentVehicle.Capacity);

                if (shipment == null || shipment.Count == 0)
                {
                    // LOG THE ERROR: Identify the specific packages that are un-shippable
                    foreach (var unShippable in remainingPackages)
                    {
                        FileLogger.Log(
                            $"DEADLOCK: Package {unShippable.Id} weight ({unShippable.Weight}kg) " +
                            $"exceeds vehicle capacity ({currentVehicle.Capacity}kg).",
                            LogLevel.Error);
                    }

                    // CRITICAL: Exit the loop so the program doesn't hang
                    break;
                }
                FileLogger.Log($"Vehicle available at {currentVehicle.AvailableTime} hrs picking up {shipment.Count} packages.");

                // Logic for successful shipment...
                foreach (var pkg in shipment)
                {
                    decimal time = currentVehicle.AvailableTime + ((decimal)pkg.Distance / currentVehicle.MaxSpeed);
                    pkg.EstimatedDeliveryTime = Math.Floor(time * 100) / 100;
                    remainingPackages.Remove(pkg);
                }

                // Update vehicle return time
                decimal maxDist = shipment.Max(p => p.Distance);
                currentVehicle.AvailableTime += (maxDist / (decimal)currentVehicle.MaxSpeed) * 2;
            }
        }

        private List<Package> GetBestShipment(List<Package> packages, int capacity)
        {
            var subsets = GetAllSubsets(packages)
                .Where(s => s.Any() && s.Sum(p => p.Weight) <= capacity)
                .ToList();

            if (!subsets.Any()) return new List<Package>();

            // PRIORITY RULES:
            // 1. Maximize Number of Packages
            // 2. Maximize Total Weight
            // 3. Prefer shipment that returns earliest (Min Max Distance)
            return subsets
                .OrderByDescending(s => s.Count)
                .ThenByDescending(s => s.Sum(p => p.Weight))
                .ThenBy(s => s.Max(p => p.Distance))
                .First();
        }

        private List<List<Package>> GetAllSubsets(List<Package> list)
        {
            var result = new List<List<Package>>();
            for (int i = 0; i < (1 << list.Count); i++)
            {
                var subset = new List<Package>();
                for (int j = 0; j < list.Count; j++)
                {
                    if ((i & (1 << j)) != 0) subset.Add(list[j]);
                }
                result.Add(subset);
            }
            return result;
        }
    }
}