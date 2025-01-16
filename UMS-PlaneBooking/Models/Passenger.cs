namespace UMS_PlaneBooking.Models;

public class Passenger
{
    public int BookingID { get; set; }
    public string PassengerName { get; set; }
    public int FlightID { get; set; }
    public Flight Flight { get; set; }
}
