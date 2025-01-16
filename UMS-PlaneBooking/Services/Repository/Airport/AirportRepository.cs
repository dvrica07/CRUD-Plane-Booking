using UMS_PlaneBooking.Models.Common;
using UMS_PlaneBooking.Repository.Entities;
using UMS_PlaneBooking.Repository.Interfaces;
using UMS_PlaneBooking.Services.Repository.Interfaces;

namespace UMS_PlaneBooking.Services.Repository.Airport;

public class AirportRepository: IAirportRepository
{
    private readonly IDataStore dataStore;
    public AirportRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public async Task<AppResult<AirportEntity>> CreateAsync(AirportEntity airport)
    {
        try
        {
            var result = await dataStore.Airport.Add(airport);
            if (!result.Succeeded || result.Result is null)
            {
                return AppResult<AirportEntity>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            var createdCustomer = result.Result;
            return AppResult<AirportEntity>.CreateSucceeded(new AirportEntity
            {
                Address     = createdCustomer.Address,
                AirportID   = createdCustomer.AirportID,
                AirportName = createdCustomer.AirportName,
            }, "Successfully created airport data");

        }
        catch (Exception ex)
        {
            return AppResult<AirportEntity>.CreateFailed(ex, "An error occured in creating airport data");
        }
    }

    public async Task<AppResult<AirportEntity>> DeleteAsync(int id)
    {
        try
        {
            var result = await dataStore.Airport.GetByIdAsync(id);

            if (!result.Succeeded || result.Result is null)
            {
                return AppResult<AirportEntity>.CreateFailed(new ApplicationException("Airport not found"), "Airport with the given ID does not exist.");
            }

            var airportToDelete = result.Result;

            var deleteResult = await dataStore.Airport.Remove(airportToDelete);

            if (!deleteResult.Succeeded)
            {
                return AppResult<AirportEntity>.CreateFailed(new ApplicationException(deleteResult.Message), deleteResult.Message);
            }

            return AppResult<AirportEntity>.CreateSucceeded(null, "Successfully deleted airport data.");
        }
        catch (Exception ex)
        {
            return AppResult<AirportEntity>.CreateFailed(ex, "An error occurred while deleting the airport data.");
        }
    }

    public async Task<AppResult<IEnumerable<AirportEntity>>> GetAllAsAsync()
    {
        try
        {
            var result = await dataStore.Airport.GetAllAsync();

            if (!result.Succeeded || result.Result is null || !result.Result.Any())
            {
                return AppResult<IEnumerable<AirportEntity>>.CreateFailed(new ApplicationException("No airports found."), "No airport records available.");
            }

            return AppResult<IEnumerable<AirportEntity>>.CreateSucceeded(result.Result, "Successfully retrieved airport data.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<AirportEntity>>.CreateFailed(ex, "An error occurred while retrieving airport data.");
        }
    }

    public async Task<AppResult<AirportEntity>> GetByName(string name)
    {
        try
        {
            var result = await dataStore.Airport.GetAirportName(name);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<AirportEntity>.CreateFailed(result.Error.Exception, result.Message);
            }

            var airportDTO = new AirportEntity
            {
                AirportID   = result.Result.AirportID,
                AirportName = result.Result.AirportName,
                Address     = result.Result.Address,
            };

            return AppResult<AirportEntity>.CreateSucceeded(airportDTO, "Successfully getting airport by name");
        }
        catch (Exception ex)
        {
            return AppResult<AirportEntity>.CreateFailed(ex, "An error occured when getting airport by name");
        }
    }

    public async Task<AppResult<AirportEntity>> UpdateAsync(AirportEntity airport)
    {
        try
        {
            var result = await dataStore.Airport.GetByIdAsync(airport.AirportID);

            if (!result.Succeeded || result.Result is null)
            {
                return AppResult<AirportEntity>.CreateFailed(new ApplicationException("Airport not found"), "Airport with the given ID does not exist.");
            }

            var existingAirport = result.Result;

            existingAirport.AirportName = airport.AirportName;
            existingAirport.Address = airport.Address;
            existingAirport.ChangedOn = DateTime.UtcNow; 

            var updateResult = await dataStore.Airport.Update(existingAirport);

            if (!updateResult.Succeeded)
            {
                return AppResult<AirportEntity>.CreateFailed(new ApplicationException(updateResult.Message), updateResult.Message);
            }

            return AppResult<AirportEntity>.CreateSucceeded(existingAirport, "Successfully updated airport data.");
        }
        catch (Exception ex)
        {
            return AppResult<AirportEntity>.CreateFailed(ex, "An error occurred while updating the airport data.");
        }
    }
}
