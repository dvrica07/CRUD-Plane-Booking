using System.ComponentModel.DataAnnotations;

namespace UMS_PlaneBooking.Models;

public class Passenger
{
    [Key]
    public int BookingID { get; set; }
    [Required(ErrorMessage = "Passenger name is required.")]
    public string PassengerName { get; set; }
    public int FlightID { get; set; }
    public Flight Flight { get; set; }
}
