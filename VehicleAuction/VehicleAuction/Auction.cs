namespace VehicleAuction.VehicleAuction;

public class Auction(Vehicle vehicle, decimal startingPrice, decimal reservePrice)
{
    public Vehicle Vehicle { get; } = vehicle;
    public decimal StartingPrice { get; } = startingPrice;
    public decimal ReservePrice { get; } = reservePrice;
    public decimal HighestBid { get; } = startingPrice;
}