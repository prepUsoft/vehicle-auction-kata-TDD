using FluentAssertions;
using VehicleAuction.VehicleAuction;
using VehicleAuctionTests.Fixtures.Vehicles;

namespace VehicleAuctionTests.VehicleAuction;

public class AuctionTests
{
    private const decimal AnyPrice = 200;
    private const string AnyName = "anyName";

    [Fact]
    public void Constructor_ShouldBeInstantiatedCorrectly()
    {
        const decimal expectedStartingPrice = 100;
        const decimal expectedReservePrice = 150;
        var auction = new Auction(VehicleFixture.Vehicle, expectedStartingPrice, expectedReservePrice);

        auction.Vehicle.Should().Be(VehicleFixture.Vehicle);
        auction.StartingPrice.Should().Be(expectedStartingPrice);
        auction.ReservePrice.Should().Be(expectedReservePrice);
    }

    [Fact]
    public void HighestBid_ShouldBeStartingPriceByDefault()
    {
        const decimal expectedStartingPrice = 100;
        var auction = new Auction(VehicleFixture.Vehicle, expectedStartingPrice, AnyPrice);

        auction.HighestBid.Should().Be(expectedStartingPrice);
    }

    [Fact]
    public void PlaceBid_ShouldReplaceHighestBidWhenBidAmountIsHigherThanCurrentHighestBid()
    {
        const decimal currentHighestBid = 100;
        const decimal higherBid = 150;
        
        var auction = new Auction(VehicleFixture.Vehicle, currentHighestBid, AnyPrice);

        auction.PlaceBid(new Bid(AnyName, higherBid));

        auction.HighestBid.Should().Be(higherBid);
    }
    
    [Theory]
    [InlineData(100)] // Equal
    [InlineData(50)] // Lower
    public void PlaceBid_ShouldThrowWhenBidAmountIsLowerOrEqualToHighestAmountAndNotChangeHighestAmount(decimal newInvalidBidAmount)
    {
        const decimal currentHighestBid = 100;
        
        var auction = new Auction(VehicleFixture.Vehicle, currentHighestBid, AnyPrice);

        var act = () => auction.PlaceBid(new Bid(AnyName, newInvalidBidAmount));

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("The bid amount must be higher than the current highest bid");
        auction.HighestBid.Should().Be(currentHighestBid);
    }

    [Fact]
    public void PlaceBid_ShouldThrowWhenTheAuctionIsClosed()
    {
        const decimal higherBid = AnyPrice + 1;
        var auction = new Auction(VehicleFixture.Vehicle, AnyPrice, AnyPrice);
        auction.PlaceBid(new Bid(AnyName, higherBid));
        
        auction.Close();

        var act = () => auction.PlaceBid(new Bid(AnyName, higherBid + 1));

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("You cannot bid when the auction is closed");
    }


    [Theory]
    [InlineData(100)]
    [InlineData(101)]
    public void Close_ShouldReturnHighestBidWhenItMeetsOrExceedsTheReservePrice(decimal bidAmount)
    {
        const decimal startingPrice = 0;
        const decimal reservePrice = 100;
        var auction = new Auction(VehicleFixture.Vehicle, startingPrice, reservePrice);
        auction.PlaceBid(new Bid(AnyName, bidAmount));

        var result = auction.Close();
            
        result.Value.Should().Be(auction.HighestBid);
    }

    [Fact]
    public void Close_ShouldThrowWhenHighestBidIsLowerThanTheReservePrice()
    {
        const decimal startingPrice = 0;
        const decimal reservePrice = 100;
        const decimal lowerHighestBid = reservePrice - 1;
        var auction = new Auction(VehicleFixture.Vehicle, startingPrice, reservePrice);
        auction.PlaceBid(new Bid(AnyName, lowerHighestBid));

        var act = () => auction.Close();

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("The reserve price must be respected before closing the auction");
    }

    [Fact]
    public void GetBidHistory_ReturnBidListInDescendingOrder()
    {
        //Arrange
        var lowestBid = new Bid("lowestBid", AnyPrice + 1);
        var mediumBid = new Bid("mediumBid", lowestBid.Value + 1);
        var highestBid = new Bid("highestBid", mediumBid.Value + 1);
        var expectedBidHistory = new List<Bid>(){highestBid,mediumBid,lowestBid};
        var auction = new Auction(VehicleFixture.Vehicle, AnyPrice, AnyPrice);
        auction.PlaceBid(lowestBid);
        auction.PlaceBid(mediumBid);
        auction.PlaceBid(highestBid);
        
        //Act
        var bidHistory = auction.GetBidHistory();
        
        //Asserts
        bidHistory.Should().Equal(expectedBidHistory);

    }

    [Fact]
    public void RemoveBids_ShouldRemoveBidsForGivenBidder()
    {
        //Arrange
        var lowestBid = new Bid("lowestBid", AnyPrice + 1);
        var mediumBid = new Bid("mediumBid", lowestBid.Value + 1);
        var mediumBidSecondBid = new Bid("mediumBid", mediumBid.Value + 1);
        var highestBid = new Bid("highestBid", mediumBidSecondBid.Value + 1);

        
        var expectedBidHistory = new List<Bid>(){highestBid,lowestBid};
        var auction = new Auction(VehicleFixture.Vehicle, AnyPrice, AnyPrice);
        auction.PlaceBid(lowestBid);
        auction.PlaceBid(mediumBid);
        auction.PlaceBid(mediumBidSecondBid);
        auction.PlaceBid(highestBid);
        
        // Act
        auction.RemoveBids("mediumBid");
        
        // Assert
        var bidHistory = auction.GetBidHistory();
        bidHistory.Should().Equal(expectedBidHistory);
    }

    [Fact]
    public void RemoveBids_ShouldThrowWhenAuctionIsClosed()
    {
        // Arrange
        var auction = new Auction(VehicleFixture.Vehicle, AnyPrice, AnyPrice);
        auction.PlaceBid(new Bid(AnyName, AnyPrice + 1));
        auction.Close();
        
        // Act
        var act = () => auction.RemoveBids(AnyName);
        
        // Assert
        act.Should().Throw<InvalidOperationException>().And.Message.Should()
            .Be("You cannot withdraw bids when the auction is closed");
    }

    [Fact]
    public void Close_ShouldReturnBidderNameAndAmountResult()
    {
        // Arrange
        var bid = new Bid(AnyName, AnyPrice + 1);
        var auction = new Auction(VehicleFixture.Vehicle, AnyPrice, AnyPrice);
        auction.PlaceBid(bid);
        
        // Act
        var result = auction.Close();
        
        // Assert
        result.BidderName.Should().Be(bid.BidderName);
        result.Value.Should().Be(bid.Value);
    }

    [Fact]
    public void Close_ShouldThrowWhenThereIsNoBid()
    {
        // Arrange
        var auction = new Auction(VehicleFixture.Vehicle, AnyPrice, AnyPrice);

        // Act
        var act = () => auction.Close();
        
        // Assert
        act.Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("There must be at least one bid to close the auction");
    }
}
 