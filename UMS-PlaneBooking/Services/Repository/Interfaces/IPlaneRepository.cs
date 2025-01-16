using UMS_PlaneBooking.Models.Common;
using UMS_PlaneBooking.Repository.Entities;

namespace UMS_PlaneBooking.Services.Repository.Interfaces;

public interface IPlaneRepository
{
    Task<AppResult<IEnumerable<PlaneEntity>>> GetAllAsAsync();
    Task<AppResult<PlaneEntity>> CreateAsync(PlaneEntity planeEntity);
    Task<AppResult<PlaneEntity>> UpdateAsync(PlaneEntity planeEntity);
    Task<AppResult<PlaneEntity>> DeleteAsync(int Id);
}
