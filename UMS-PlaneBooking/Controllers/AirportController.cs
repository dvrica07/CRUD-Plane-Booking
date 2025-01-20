using Microsoft.AspNetCore.Mvc;
using UMS_PlaneBooking.Models;
using UMS_PlaneBooking.Services.Repository.Interfaces;

namespace UMS_PlaneBooking.Controllers;

public class AirportController : Controller
{
    private readonly IAirportRepository _airportRepository;
    public AirportController(IAirportRepository airportRepository)
    {
        _airportRepository = airportRepository;
    }
    public async Task<IActionResult> Index()
    {
        var result = await _airportRepository.GetAllAsAsync();
        var airports = result.Result.Select(a => new Models.Airport
        {
            AirportID = a.AirportID,
            AirportName = a.AirportName,
            Address = a.Address
        }).ToList();
        return View(airports); 
    }
    // Get: Check if airport by name exists
    public async Task<IActionResult> GetByAirportName(string airportName)
    {
        try
        {
            var result = await _airportRepository.GetByName(airportName);  
            if (result.Result != null)
            {
                return Json(new { exists = true });
            }
            return Json(new { exists = false });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
        }
    }
    //Create 
    [HttpPost]
    public async Task<IActionResult> AddAirport([FromBody] Airport airport)
    {
        try
        {
            var create = await _airportRepository.CreateAsync(new Repository.Entities.AirportEntity
            {
                Address = airport.Address,
                AirportName = airport.AirportName,
                ChangedOn = DateTime.Now,
                CreatedOn = DateTime.Now,
            });

            if (create.Succeeded && create.Result is not null)
            {
                return Ok(new { message = "Airport added successfully." });
            }

            return BadRequest(new { message = "Failed to add airport." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
        }
    }
    //Update
    [HttpPost]
    public async Task<IActionResult> UpdateAirport([FromBody] Airport airport)
    {
        try
        {
            var update = await _airportRepository.UpdateAsync(new Repository.Entities.AirportEntity
            {
                AirportID   = airport.AirportID,
                Address     = airport.Address,
                AirportName = airport.AirportName,
                ChangedOn   = DateTime.Now,
            });

            if (update.Succeeded && update.Result is not null)
            {
                return Ok(new { message = "Airport updated successfully." });
            }

            return BadRequest(new { message = "Failed to update airport." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
        }
    }
    //Delete
    [HttpPost]
    public async Task<IActionResult> DeleteAirport([FromBody] Airport airport)
    {
        try
        {
            var delete = await _airportRepository.DeleteAsync(airport.AirportID);
            if (delete.Succeeded)
            {
                return Ok(new { message = "Airport deleted successfully." });
            }

            return BadRequest(new { message = "Failed to delete airport." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
        }
    }
}
