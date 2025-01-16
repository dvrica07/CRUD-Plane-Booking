namespace UMS_PlaneBooking.Repository.Interfaces;
public interface IDataStore
{
    IAirport Airport { get; }
    IFlight Flight { get; }
    IPassenger Passenger { get; }
    IPlane Plane { get; }
    Task EnsureMigrate();
    Task SeedData();
}
