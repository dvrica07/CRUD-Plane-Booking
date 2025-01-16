using UMS_PlaneBooking.Models.Common;
using UMS_PlaneBooking.Repository.Entities;

namespace UMS_PlaneBooking.Services.Repository.Interfaces;

public interface IPassengerRepository
{
    Task<AppResult<IEnumerable<PassengerEntity>>> GetAllAsAsync();
    Task<AppResult<PassengerEntity>> CreateAsync(PassengerEntity passengerEntity);
    Task<AppResult<PassengerEntity>> UpdateAsync(PassengerEntity passengerEntity);
    Task<AppResult<PassengerEntity>> DeleteAsync(int Id);

}
