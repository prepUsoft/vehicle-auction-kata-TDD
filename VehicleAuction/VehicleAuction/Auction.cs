namespace VehicleAuction.VehicleAuction;

public class Auction(Vehicle vehicle, decimal startingPrice, decimal reservePrice)
{
    public Vehicle Vehicle { get; } = vehicle;
    public decimal StartingPrice { get; } = startingPrice;
    public decimal ReservePrice { get; } = reservePrice;
    public decimal HighestBid { get; private set; } = startingPrice;

    public void PlaceBid(string name, decimal bid)
    {
        if (bid > HighestBid)
            HighestBid = bid;

        throw new InvalidOperationException("The bid amount must be higher than the current highest bid");
    }
}