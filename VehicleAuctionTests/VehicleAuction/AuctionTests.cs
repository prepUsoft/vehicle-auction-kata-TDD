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
}