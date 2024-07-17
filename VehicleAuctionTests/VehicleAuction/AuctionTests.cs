using FluentAssertions;
using VehicleAuction.VehicleAuction;
using VehicleAuctionTests.Fixtures.Vehicles;

namespace VehicleAuctionTests.VehicleAuction;

public class AuctionTests
{
    [Fact]
    public void Constructor_ShouldBeInstantiatedCorrectly()
    {
        var auction = new Auction(VehicleFixture.Vehicle);

        auction.Vehicle.Should().Be(VehicleFixture.Vehicle);
    }
}