using System.ComponentModel.DataAnnotations;

namespace UMS_PlaneBooking.Models;

public class Airport
{
    [Key]
    public int AirportID { get; set; }

    [Required(ErrorMessage = "Airport name is required.")]
    public string AirportName { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    public string Address { get; set; }
    public ICollection<Flight> Flights { get; set; }
}
