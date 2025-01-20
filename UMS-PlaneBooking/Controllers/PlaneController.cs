using Microsoft.AspNetCore.Mvc;
using UMS_PlaneBooking.Models;
using UMS_PlaneBooking.Services.Repository.Interfaces;

namespace UMS_PlaneBooking.Controllers
{
    public class PlaneController : Controller
    {
        private readonly IPlaneRepository _planeRepository;
        public PlaneController(IPlaneRepository planeRepository)
        {
            _planeRepository = planeRepository;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _planeRepository.GetAllAsAsync();
            var planes = result.Result.Select(b => new Models.Plane
            {
                PlaneID = b.PlaneID,
                PlaneCode = b.PlaneCode,    
                Airline = b.Airline,
                Model = b.Model,
            }).ToList();
            return View(planes);
        }
        public async Task<IActionResult>AddPlane([FromBody] Plane plane)
        {
            try
            {
                var createPlane = await _planeRepository.CreateAsync(new Repository.Entities.PlaneEntity
                {
                    Airline = plane.Airline,
                    Model = plane.Model,
                    PlaneCode = plane.PlaneCode,
                });

                if (createPlane.Succeeded && createPlane.Result is not null)
                {
                    return Ok(new { message = "Plane added successfully." });
                }

                return BadRequest(new { message = "Failed to add plane." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }
        public async Task<IActionResult> UpdatePlane([FromBody] Plane plane)
        {
            try
            {
                var update = await _planeRepository.UpdateAsync(new Repository.Entities.PlaneEntity
                {
                    PlaneID = plane.PlaneID,
                    Airline = plane.Airline,
                    Model = plane.Model,
                    ChangedOn = DateTime.Now,
                });

                if (update.Succeeded && update.Result is not null)
                {
                    return Ok(new { message = "Plane updated successfully." });
                }

                return BadRequest(new { message = "Failed to update plane." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }
        public async Task<IActionResult> DeletePlane([FromBody] Plane plane)
        {
            try
            {
                var delete = await _planeRepository.DeleteAsync(plane.PlaneID);
                if (delete.Succeeded)
                {
                    return Ok(new { message = "Plane deleted successfully." });
                }

                return BadRequest(new { message = "Failed to delete plane." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }
    }
}
