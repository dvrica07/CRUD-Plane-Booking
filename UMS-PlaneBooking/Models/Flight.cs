using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace UMS_PlaneBooking.Models;
public class Flight
{
    [Key]
    public int FlightID { get; set; }
    [Required(ErrorMessage = "Flight code is required.")]
    public string FlightCode { get; set; }
    public int AirportID { get; set; }
    public Airport Airport { get; set; }
    public int PlaneID { get; set; }
    public Plane Plane { get; set; }
    public ICollection<Passenger> PassengerBookings { get; set; }
}
