namespace VehicleAuction.VehicleAuction;

public class Auction(Vehicle vehicle, decimal startingPrice, decimal reservePrice)
{
    public Vehicle Vehicle { get; } = vehicle;
    public decimal StartingPrice { get; } = startingPrice;
    public decimal ReservePrice { get; } = reservePrice;
    public decimal HighestBid { get; private set; } = startingPrice;
    private bool _isClosed;

    private readonly List<Bid> _bidHistory = [];

    public void PlaceBid(Bid bid)
    {
        if (_isClosed)
            throw new InvalidOperationException("You cannot bid when the auction is closed");
        if (bid.Value > HighestBid)
        {
            HighestBid = bid.Value;
            _bidHistory.Add(bid);
        }
        else
            throw new InvalidOperationException("The bid amount must be higher than the current highest bid");
    }

    public Bid Close()
    {
        if (GetBidHistory().Count == 0)
            throw new InvalidOperationException("There must be at least one bid to close the auction");
        
        if (HighestBid < ReservePrice)
            throw new InvalidOperationException("The reserve price must be respected before closing the auction");

        _isClosed = true;

        return GetBidHistory().First();
    }

    public List<Bid> GetBidHistory()
    {
        return _bidHistory.OrderByDescending(x=> x.Value).ToList();
    }

    public void RemoveBids(string bidderName)
    {
        if (_isClosed)
            throw new InvalidOperationException("You cannot withdraw bids when the auction is closed");
        
        _bidHistory.RemoveAll(x=> x.BidderName == bidderName);
    }

}