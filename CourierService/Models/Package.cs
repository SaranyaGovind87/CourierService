namespace CourierService.Models
{
    public class Package
    {
        public string Id { get; init; }
        public int Weight { get; init; }
        public int Distance { get; init; }
        public string OfferCode { get; init; }

        // Calculated properties
        public decimal DiscountAmount { get; set; }
        public decimal TotalCost { get; set; }
        public decimal EstimatedDeliveryTime { get; set; }

        public Package(string id, int weight, int distance, string? offerCode)
        {
            Id = id;
            Weight = weight;
            Distance = distance;

            // ENCAPSULATED LOGIC: 
            // If offerCode is null, empty, or whitespace, default to "NA"
            OfferCode = string.IsNullOrWhiteSpace(offerCode) ? "NA" : offerCode.ToUpper().Trim();
        }
    }
}