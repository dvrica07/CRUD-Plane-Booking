using UMS_PlaneBooking.Repository.Entities;
using UMS_PlaneBooking.Models.Common;

namespace UMS_PlaneBooking.Services.Repository.Interfaces;
public interface IAirportRepository
{
    Task<AppResult<IEnumerable<AirportEntity>>> GetAllAsAsync();
    Task<AppResult<AirportEntity>> CreateAsync(AirportEntity airport);  
    Task<AppResult<AirportEntity>> UpdateAsync(AirportEntity airport);  
    Task<AppResult<AirportEntity>> DeleteAsync(int id);
    Task<AppResult<AirportEntity>> GetByName(string name);
}

