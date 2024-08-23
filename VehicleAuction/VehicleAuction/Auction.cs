namespace VehicleAuction.VehicleAuction;

public class Auction(Vehicle vehicle, decimal startingPrice, decimal reservePrice)
{
    public Vehicle Vehicle { get; } = vehicle;
    public decimal StartingPrice { get; } = startingPrice;
    public decimal ReservePrice { get; } = reservePrice;
    public decimal HighestBid { get; private set; } = startingPrice;
    private bool _isClosed = false;

    private List<Bid> BidHistory = new ();

    public void PlaceBid(Bid bid)
    {
        if (_isClosed)
            throw new InvalidOperationException("You cannot bid when the auction is closed");
        if (bid.Value > HighestBid)
        {
            HighestBid = bid.Value;
            BidHistory.Add(bid);
        }
        else
            throw new InvalidOperationException("The bid amount must be higher than the current highest bid");
    }

    public decimal Close()
    {
        if (HighestBid < reservePrice)
            throw new InvalidOperationException("The reserve price must be respected before closing the auction");

        _isClosed = true;

        return HighestBid;
    }

    public List<Bid> GetBidHistory()
    {
        return BidHistory.OrderByDescending(x=> x.Value).ToList();
    }

    public void RemoveBids(string bidderName)
    {
        BidHistory.RemoveAll(x=> x.BidderName == bidderName);
    }

}