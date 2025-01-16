using System.ComponentModel.DataAnnotations;

namespace UMS_PlaneBooking.Models;

public class Airport
{
    [Key]
    public int AirportID { get; set; }

    [Required(ErrorMessage = "Airport name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string AirportName { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
    public string Address { get; set; }
    public ICollection<Flight> Flights { get; set; }
}
