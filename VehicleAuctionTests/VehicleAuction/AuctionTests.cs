using FluentAssertions;
using VehicleAuction.VehicleAuction;
using VehicleAuctionTests.Fixtures.Vehicles;

namespace VehicleAuctionTests.VehicleAuction;

public class AuctionTests
{
    private const decimal anyPrice = 200;
    
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
        var auction = new Auction(VehicleFixture.Vehicle, expectedStartingPrice, anyPrice);

        auction.HighestBid.Should().Be(expectedStartingPrice);
    }

    [Fact]
    public void PlaceBid_ShouldReplaceHighestBidWhenBidAmountIsHigherThanCurrentHighestBid()
    {
        const decimal currentHighestBid = 100;
        const decimal higherBid = 150;
        
        var auction = new Auction(VehicleFixture.Vehicle, currentHighestBid, anyPrice);

        auction.PlaceBid("someName", higherBid);

        auction.HighestBid.Should().Be(higherBid);
    }
    
    [Theory]
    [InlineData(100)] // Equal
    [InlineData(50)] // Lower
    public void PlaceBid_ShouldThrowWhenBidAmountIsLowerOrEqualToHighestAmountAndNotChangeHighestAmount(decimal newInvalidBidAmount)
    {
        const decimal currentHighestBid = 100;
        
        var auction = new Auction(VehicleFixture.Vehicle, currentHighestBid, anyPrice);

        var act = () => auction.PlaceBid("someName", newInvalidBidAmount);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("The bid amount must be higher than the current highest bid");
        auction.HighestBid.Should().Be(currentHighestBid);
    }

    [Fact]
    public void PlaceBid_ShouldThrowWhenTheAuctionIsClosed()
    {
        const decimal higherBid = anyPrice + 1;
        var auction = new Auction(VehicleFixture.Vehicle, anyPrice, anyPrice);
        
        auction.Close();

        var act = () => auction.PlaceBid("newname", higherBid);

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
        auction.PlaceBid("anyname", bidAmount);

        var result = auction.Close();
            
        result.Should().Be(auction.HighestBid);
    }

    [Fact]
    public void Close_ShouldThrowWhenHighestBidIsLowerThanTheReservePrice()
    {
        const decimal startingPrice = 0;
        const decimal reservePrice = 100;
        const decimal lowerHighestBid = reservePrice - 1;
        var auction = new Auction(VehicleFixture.Vehicle, startingPrice, reservePrice);
        auction.PlaceBid("somename", lowerHighestBid);

        var act = () => auction.Close();

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("The reserve price must be respected before closing the auction");
    }

    [Fact]
    public void GetBidHistory_ReturnBidListInDescendingOrder()
    {
        //Arrange
        const decimal lowestBid = anyPrice + 1;
        const decimal mediumBid = lowestBid + 1;
        const decimal highestBid = mediumBid + 1;
        var expectedBidHistory = new List<decimal>(){highestBid,mediumBid,lowestBid};
        var auction = new Auction(VehicleFixture.Vehicle, anyPrice, anyPrice);
        auction.PlaceBid("lowestBid",lowestBid);
        auction.PlaceBid("mediumBid",mediumBid);
        auction.PlaceBid("highestBid", highestBid);
        
        //Act
        var bidHistory = auction.GetBidHistory();
        
        //Asserts
        bidHistory.Should().Equal(expectedBidHistory);

    }

    [Fact]
    public void WithdrawBids_ShouldRemoveBidsForGivenBidder()
    {
        //Arrange
        var lowestBid = new Bid() { BidderName = "lowestBid", Value = anyPrice + 1 };
        var mediumBid = new Bid() { BidderName = "mediumBid", Value = lowestBid.Value + 1 };
        var mediumBidSecondBid = new Bid() { BidderName = "mediumBid", Value = mediumBid.Value + 1 };
        var highestBid = new Bid() { BidderName = "highestBid", Value = mediumBidSecondBid.Value + 1 };

        
        var expectedBidHistory = new List<Bid>(){highestBid,lowestBid};
        var auction = new Auction(VehicleFixture.Vehicle, anyPrice, anyPrice);
        auction.PlaceBid(lowestBid);
        auction.PlaceBid(mediumBid);
        auction.PlaceBid(mediumBidSecondBid);
        auction.PlaceBid(highestBid);
        
        auction.RemoveBids("mediumBid");
        var bidHistory = auction.GetBidHistory();
        bidHistory.Should().Equal(expectedBidHistory);
    }

}
 