using System.ComponentModel.DataAnnotations;

namespace UMS_PlaneBooking.Models;

public class Plane
{
    [Key]
    public int PlaneID { get; set; }
    public string PlaneCode { get; set; }
    public string Airline { get; set; }
    public string Model { get; set; }
    public ICollection<Flight> Flights { get; set; }
}
