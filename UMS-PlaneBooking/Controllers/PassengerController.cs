using Microsoft.AspNetCore.Mvc;
using UMS_PlaneBooking.Models;
using UMS_PlaneBooking.Repository.Interfaces;
using UMS_PlaneBooking.Services.Repository.Interfaces;

namespace UMS_PlaneBooking.Controllers
{
    public class PassengerController : Controller
    {
        private readonly IPassengerRepository _passengerRepository;
        private readonly IFlightRepository _flightRepository;
        public PassengerController(IPassengerRepository passengerRepository, IFlightRepository flightRepository)
        {
            _passengerRepository = passengerRepository;
            _flightRepository    = flightRepository;
        }
        public async Task<IActionResult> Index()
        {
            // Initialize the passengers list
            var passengers = new List<Models.Passenger>();
            var result = await _passengerRepository.GetAllAsAsync();
            if (result.Succeeded && result.Result is not null)
            {
                passengers = result.Result.Select(c => new Models.Passenger
                {
                    BookingID = c.BookingID,
                    PassengerName = c.PassengerName,
                    Flight = new Models.Flight
                    {
                        FlightID = c.Flight.FlightID,
                        FlightCode = c.Flight.FlightCode
                    }
                }).ToList();
            }

            //Get All Available Flights
            var flightsResult = await _flightRepository.GetAllAsAsync();
            if (flightsResult.Succeeded && flightsResult.Result is not null)
            {
                var availableFlights = flightsResult.Result.Select(f => new Flight
                {
                    FlightID = f.FlightID,
                    FlightCode = f.FlightCode,
                    Airport = new Models.Airport
                    {
                        AirportID = f.Airport.AirportID,
                        AirportName = f.Airport.AirportName,
                    },
                    Plane = new Models.Plane
                    {
                        PlaneID = f.Plane.PlaneID,
                        Airline = f.Plane.Airline,
                    }
                });
                ViewData["Flights"] = availableFlights.ToList();
            }
            return View(passengers);
        }
        public async Task<IActionResult> AddPassenger([FromBody] Passenger passenger)
        {
            try
            {
                var create = await _passengerRepository.CreateAsync(new Repository.Entities.PassengerEntity
                {
                    PassengerName = passenger.PassengerName,
                    FlightID      = passenger.FlightID
                });
                if (create.Succeeded && create.Result is not null)
                {
                    return Ok(new { message = "Passenger added successfully." });
                }

                return BadRequest(new { message = "Failed to add passenger." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }
        public async Task<IActionResult> UpdatePassenger([FromBody] Passenger passenger)
        {
            try
            {
                var update = await _passengerRepository.UpdateAsync(new Repository.Entities.PassengerEntity
                {
                    BookingID     = passenger.BookingID,
                    PassengerName = passenger.PassengerName,
                    FlightID      = passenger.FlightID,
                    ChangedOn     = DateTime.Now
                });

                if (update.Succeeded && update.Result is not null)
                {
                    return Ok(new { message = "Passenger updated successfully." });
                }

                return BadRequest(new { message = "Failed to update passenger." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }
        public async Task<IActionResult> DeletePassenger([FromBody] Passenger passenger)
        {
            try
            {
                var delete = await _passengerRepository.DeleteAsync(passenger.BookingID);
                if (delete.Succeeded)
                {
                    return Ok(new { message = "Passenger deleted successfully." });
                }

                return BadRequest(new { message = "Failed to delete passenger." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }
    }
}
