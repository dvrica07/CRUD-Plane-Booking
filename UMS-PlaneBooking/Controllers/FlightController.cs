using Microsoft.AspNetCore.Mvc;
using UMS_PlaneBooking.Models;
using UMS_PlaneBooking.Services.Repository.Interfaces;

namespace UMS_PlaneBooking.Controllers
{
    public class FlightController : Controller
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IAirportRepository _airportRepository;
        private readonly IPlaneRepository _planeRepository;
        public FlightController(IFlightRepository flightRepository, IAirportRepository airportRepository, IPlaneRepository planeRepository)
        {
            _flightRepository  = flightRepository;
            _airportRepository = airportRepository;
            _planeRepository   = planeRepository;
        }
        public async Task<IActionResult> Index()
        {
            List<Flight> flights = new List<Flight>();  
            var result = await _flightRepository.GetAllAsAsync();
            if (result.Succeeded && result.Result is not null)
            {
                flights = result.Result.Select(b => new Models.Flight
                {
                    FlightID   = b.FlightID,
                    FlightCode = b.FlightCode,
                    Airport = new Models.Airport
                    {
                        AirportID   = b.Airport.AirportID,
                        AirportName = b.Airport.AirportName,
                    },
                    Plane = new Models.Plane
                    {
                        PlaneID = b.Plane.PlaneID,
                        Airline = b.Plane.Airline,
                    }
                }).ToList();
            }
            
            //Get All Available Airports
            var airportResult = await _airportRepository.GetAllAsAsync();
            if (airportResult.Succeeded && airportResult.Result is not null)
            {
                var airports = airportResult.Result.Select(a => new Models.Airport
                {
                    AirportID   = a.AirportID,
                    AirportName = a.AirportName
                }).ToList();

                ViewData["Airports"] = airports;
            }

            //Get All Available Planes
            var planeResult = await _planeRepository.GetAllAsAsync();
            if (planeResult.Succeeded && planeResult.Result is not null)
            {
                var planes = planeResult.Result.Select(b => new Models.Plane
                {
                    PlaneID   = b.PlaneID,
                    PlaneCode = b.PlaneCode,
                    Airline   = b.Airline,
                    Model     = b.Model,
                }).ToList();

                ViewData["Planes"] = planes;
            }
            return View(flights);
        }
        [HttpPost]
        public async Task<IActionResult> AddFlight([FromBody] Flight flight)
        {
            try
            {
                var create = await _flightRepository.CreateAsync(new Repository.Entities.FlightsEntity
                {
                    FlightCode = flight.FlightCode,
                    AirportID  = flight.AirportID,
                    PlaneID    = flight.PlaneID,
                });
                if (create.Succeeded && create.Result is not null)
                {
                    return Ok(new { message = "Flight created successfully." });
                }

                return BadRequest(new { message = "Failed to create flight." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateFlight([FromBody] Flight flight)
        {
            try
            {
                var create = await _flightRepository.UpdateAsync(new Repository.Entities.FlightsEntity
                {
                    FlightID   = flight.FlightID,
                    FlightCode = flight.FlightCode,
                    AirportID  = flight.AirportID,
                    PlaneID    = flight.PlaneID,
                    ChangedOn  = DateTime.Now,
                });

                if (create.Succeeded && create.Result is not null)
                {
                    return Ok(new { message = "Flight updated successfully." });
                }

                return BadRequest(new { message = "Failed to update flight." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }
        //Delete
        [HttpPost]
        public async Task<IActionResult> DeleteFlight([FromBody] Flight flight)
        {
            try
            {
                var delete = await _flightRepository.DeleteAsync(flight.FlightID);
                if (delete.Succeeded)
                {
                    return Ok(new { message = "Flight deleted successfully." });
                }

                return BadRequest(new { message = "Failed to delete flight." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }
    }
}
