namespace UMS_PlaneBooking.Repository.Entities;
public class PassengerEntity: BaseEntity
{
    public int BookingID { get; set; }
    public string PassengerName { get; set; }
    public int FlightID { get; set; }
    public FlightsEntity Flight { get; set; }
}
