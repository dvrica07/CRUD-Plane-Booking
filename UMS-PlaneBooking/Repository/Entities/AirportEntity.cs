namespace UMS_PlaneBooking.Repository.Entities;
public class AirportEntity : BaseEntity
{
    public int AirportID { get; set; }
    public string AirportName { get; set; }
    public string Address { get; set; }
    public ICollection<FlightsEntity> Flights { get; set; }
}
