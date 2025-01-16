namespace UMS_PlaneBooking.Repository.Entities;

public class FlightsEntity : BaseEntity
{
    public int FlightID { get; set; }
    public string FlightCode { get; set; }
    public int AirportID { get; set; }
    public int PlaneID { get; set; }
    public AirportEntity Airport { get; set; }
    public PlaneEntity Plane { get; set; }
    public ICollection<PassengerEntity> Passengers{ get; set; }
}
