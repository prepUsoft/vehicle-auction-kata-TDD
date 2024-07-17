namespace VehicleAuction.VehicleAuction;

public class Vehicle(string make, string model, int year)
{
    public string Make { get; } = make;
    public string Model { get; } = model;
    public int Year { get; } = year;
}