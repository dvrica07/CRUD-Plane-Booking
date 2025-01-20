using System.ComponentModel.DataAnnotations;

namespace UMS_PlaneBooking.Models;

public class Plane
{
    [Key]
    public int PlaneID { get; set; }
    [Required(ErrorMessage = "Plane code is required.")]
    public string PlaneCode { get; set; }
    [Required(ErrorMessage = "Airline code is required.")]
    public string Airline { get; set; }
    public string Model { get; set; }
    public ICollection<Flight> Flights { get; set; }
}
