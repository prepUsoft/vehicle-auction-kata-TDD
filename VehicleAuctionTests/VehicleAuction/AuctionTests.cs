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
}