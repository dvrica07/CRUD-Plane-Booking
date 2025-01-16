using UMS_PlaneBooking.Models.Common;
using UMS_PlaneBooking.Repository.Entities;

namespace UMS_PlaneBooking.Repository.Interfaces;

public interface IAirport : IGenericEntity<AirportEntity>
{
    Task<AppResult<AirportEntity>>GetAirportName(string Name);
}
