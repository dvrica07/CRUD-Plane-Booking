using UMS_PlaneBooking.Repository.Entities;
using UMS_PlaneBooking.Models.Common;

namespace UMS_PlaneBooking.Services.Repository.Interfaces;
public interface IFlightRepository
{
    Task<AppResult<IEnumerable<FlightsEntity>>> GetAllAsAsync();
    Task<AppResult<FlightsEntity>> CreateAsync(FlightsEntity flightsEntity);
    Task<AppResult<FlightsEntity>> UpdateAsync(FlightsEntity flightsEntity);
    Task<AppResult<FlightsEntity>> DeleteAsync(int Id);
}
