using System.Linq.Expressions;
using UMS_PlaneBooking.Models;
using UMS_PlaneBooking.Models.Common;
using UMS_PlaneBooking.Repository.Entities;
using UMS_PlaneBooking.Repository.Interfaces;
using UMS_PlaneBooking.Services.Repository.Interfaces;

namespace UMS_PlaneBooking.Services.Repository.Passenger;

public class PassengerRepository : IPassengerRepository
{
    private readonly IDataStore dataStore;
    public PassengerRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public async Task<AppResult<PassengerEntity>> CreateAsync(PassengerEntity passengerEntity)
    {
        try
        {
            var result = await dataStore.Passenger.Add(passengerEntity);
            if (!result.Succeeded || result.Result is null)
            {
                return AppResult<PassengerEntity>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            var createdPassenger = result.Result;
            return AppResult<PassengerEntity>.CreateSucceeded(new PassengerEntity
            {
                PassengerName = passengerEntity.PassengerName,
                FlightID = passengerEntity.FlightID,
                BookingID = passengerEntity.BookingID,
                Flight = passengerEntity.Flight,
            }, "Successfully created passenger data");

        }
        catch (Exception ex)
        {
            return AppResult<PassengerEntity>.CreateFailed(ex, "An error occured in creating passenger data");
        }
    }

    public async Task<AppResult<PassengerEntity>> DeleteAsync(int Id)
    {
        try
        {
            // Retrieve the flight entity by ID
            var result = await dataStore.Passenger.GetByIdAsync(Id);

            if (!result.Succeeded || result.Result is null)
            {
                return AppResult<PassengerEntity>.CreateFailed(new ApplicationException("Passenger not found"), "Passenger with the given ID does not exist.");
            }

            var passengerToDelete = result.Result;

            var deleteResult = await dataStore.Passenger.Remove(passengerToDelete);
            if (!deleteResult.Succeeded || deleteResult.Result is null)
            {
                return AppResult<PassengerEntity>.CreateFailed(new ApplicationException(deleteResult.Message), deleteResult.Message);
            }

            return AppResult<PassengerEntity>.CreateSucceeded(null, "Successfully deleted passenger data");
        }
        catch (Exception ex)
        {
            return AppResult<PassengerEntity>.CreateFailed(ex, "An error occurred while deleting passenger data");
        }
    }

    public async Task<AppResult<IEnumerable<PassengerEntity>>> GetAllAsAsync()
    {
        try
        {
            //Options if you want to add flight details
            bool includeFlight = true;
            var includes = new List<Expression<Func<PassengerEntity, object>>>();
            if (includeFlight) includes.Add(a => a.Flight);

            Expression<Func<PassengerEntity, bool>> filter = a => a.FlightID != null;

            //You can add skip and take
            var result = await dataStore.Passenger.FindAsync(filter, null, null, includes);

            if (!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<PassengerEntity>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<IEnumerable<PassengerEntity>>.CreateSucceeded(result.Result, "Successfully retrieved all passengers");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<PassengerEntity>>.CreateFailed(ex, "An error occurred while retrieving passengers data");
        }
    }

    public async Task<AppResult<PassengerEntity>> UpdateAsync(PassengerEntity passengerEntity)
    {
        try
        {
            var result = await dataStore.Passenger.GetByIdAsync(passengerEntity.BookingID);

            if (!result.Succeeded || result.Result is null)
            {
                return AppResult<PassengerEntity>.CreateFailed(new ApplicationException("Passenger not found"), "Passenger with the given ID does not exist.");
            }

            var existingFlight           = result.Result;
            existingFlight.PassengerName = passengerEntity.PassengerName;
            existingFlight.FlightID      = passengerEntity.FlightID;
            existingFlight.BookingID     = passengerEntity.BookingID;
            existingFlight.ChangedOn     = passengerEntity.ChangedOn;


            var updateResult = await dataStore.Passenger.Update(existingFlight);
            if (!result.Succeeded || result.Result is null)
            {
                return AppResult<PassengerEntity>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<PassengerEntity>.CreateSucceeded(result.Result, "Successfully updated passenger data");
        }
        catch (Exception ex)
        {
            return AppResult<PassengerEntity>.CreateFailed(ex, "An error occurred while updating passenger data");
        }
    }
}
