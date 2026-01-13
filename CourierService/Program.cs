using CourierService.Logging;
using CourierService.Models;
using CourierService.Services;
using CourierService.Strategies;
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main(string[] args)
    {
        Console.Clear();
        LoadPrompt();        
    }

    private static void LoadPrompt()
    {
        Console.Title = "Everest Engineering Logistics System";
        PrintHeader();
        try
        {
            // 1. Get Base Delivery Cost and Package Count
            Console.Write("Enter [Base Delivery Cost] and [Number of Packages] (e.g. 100 5): ");
            var initialInput = Console.ReadLine()?.Split(' ');
            if (initialInput?.Length < 2) throw new Exception("Invalid initial input.");

            decimal baseCost = decimal.Parse(initialInput[0]);
            int packageCount = int.Parse(initialInput[1]);

            List<Package> packages = new();

            // 2. Guided Package Input
            for (int i = 0; i < packageCount; i++)
            {
                Console.WriteLine($"\n--- Data for Package {i + 1} of {packageCount} ---");
                Console.Write("Enter [ID] [Weight] [Distance] [OfferCode] (e.g. PKG1 50 30 OFR001): ");

                var pkgInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(pkgInput)) continue;

                var pkgData = pkgInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (pkgData?.Length < 3)
                {
                    FileLogger.Log($"Invalid package data for entry {i + 1}. Minimum 3 values required.", LogLevel.Warning);
                    continue;
                }

                try
                {
                    string id = pkgData[0];
                    int weight = int.Parse(pkgData[1]);
                    int distance = int.Parse(pkgData[2]);

                    // Pass the 4th element if it exists, otherwise pass null.
                    // The Package constructor will automatically convert null/empty to "NA".
                    string? offerCodeInput = pkgData.Length > 3 ? pkgData[3] : null;

                    packages.Add(new Package(id, weight, distance, offerCodeInput));

                    FileLogger.Log($"Added {id} successfully.");
                }
                catch (FormatException)
                {
                    FileLogger.Log("Weight and Distance must be numeric values. Skipping package.", LogLevel.Error);
                }
            }

            // 3. Guided Fleet Input
            Console.WriteLine("\n--- Fleet Configuration ---");
            Console.Write("Enter [No. of Vehicles] [Max Speed] [Max Load] (e.g. 2 70 200): ");
            var fleetData = Console.ReadLine()?.Split(' ');

            int noOfVehicles = int.Parse(fleetData[0]);
            int maxSpeed = int.Parse(fleetData[1]);
            int maxLoad = int.Parse(fleetData[2]);

            List<Vehicle> vehicles = new();
            for (int i = 1; i <= noOfVehicles; i++) vehicles.Add(new Vehicle(i, maxSpeed, maxLoad));

            // 4. Processing Vehical Duration 
            Console.WriteLine("\nCalculating optimal delivery schedule...");
            var fleetService = new FleetService();
            fleetService.CalculateDeliveryTimes(packages, vehicles);

            //5. Processing Delivery Cost and Discount
            var strategies = new List<IOfferStrategy>
            {
                new Offer001(), new Offer002(), new Offer003()
            };
            var deliveryCostService = new DeliveryCostService(strategies);
            foreach (var pkg in packages)
            {
                deliveryCostService.CalculateCost(pkg, baseCost);
            }

            // 5. Final Table Output
            PrintResults(packages);
        }
        catch (Exception ex)
        {
            FileLogger.Log($"Application Error: {ex.Message}", LogLevel.Error);
            Console.WriteLine("A critical error occurred. Please check 'delivery_log.txt' for details.");
        }
        Console.WriteLine("\nPress 'Y' to Estimate again.. ");
        Console.WriteLine("\nPress 'N' to Exit.. ");
        var reTrigger = Console.ReadLine();
        if (reTrigger != null && reTrigger.ToUpper() == "Y") {
            LoadPrompt();
        }
        else { Console.ReadKey(); }
            
    }
    private static void PrintHeader()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("=============================================");
        Console.WriteLine("    COURIER SERVICE - DELIVERY ESTIMATOR     ");
        Console.WriteLine("=============================================");
        Console.ResetColor();
    }

    private static void PrintResults(List<Package> packages)
    {
        Console.WriteLine("\n" + new string('-', 60));
        Console.WriteLine(string.Format("{0,-10} {1,-15} {2,-15} {3,-10}", "ID", "Discount", "Total Cost", "Delivery Time"));
        Console.WriteLine(new string('-', 60));

        foreach (var pkg in packages)
        {
            // Assuming Cost Logic is already handled in your CostService
            Console.WriteLine(string.Format("{0,-10} {1,-15} {2,-15} {3,-10}",
                pkg.Id, pkg.DiscountAmount, pkg.TotalCost, pkg.EstimatedDeliveryTime));
        }
    }
}