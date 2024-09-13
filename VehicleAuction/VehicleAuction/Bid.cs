namespace VehicleAuction.VehicleAuction;

public class Bid(string bidderName, decimal value)
{
    public string BidderName { get; } = bidderName;
    public decimal Value { get; } = value;
}