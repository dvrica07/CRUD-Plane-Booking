namespace UMS_PlaneBooking.Repository.Entities;

public class PlaneEntity : BaseEntity
{
    public int PlaneID { get; set; }
    public string PlaneCode { get; set; }
    public string Airline { get; set; }
    public string Model { get; set; }
    public ICollection<FlightsEntity> Flights { get; set; }
}
