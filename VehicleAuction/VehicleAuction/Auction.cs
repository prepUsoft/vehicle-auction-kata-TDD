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
        else
            throw new InvalidOperationException("The bid amount must be higher than the current highest bid");
    }

    public decimal Close()
    {
        if (HighestBid < reservePrice)
            throw new InvalidOperationException("The reserve price must be respected before closing the auction");
        
        return HighestBid;
    }
}