using UMS_PlaneBooking.Models;
using System.Linq.Expressions;
using UMS_PlaneBooking.Models.Common;
using UMS_PlaneBooking.Repository.Entities;
using UMS_PlaneBooking.Repository.Interfaces;
using UMS_PlaneBooking.Services.Repository.Interfaces;

namespace UMS_PlaneBooking.Services.Repository.Flight;

public class FlightRepository : IFlightRepository
{
    private readonly IDataStore dataStore;
    public FlightRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public async Task<AppResult<FlightsEntity>> CreateAsync(FlightsEntity flightsEntity)
    {
        try
        {
            var result = await dataStore.Flight.Add(flightsEntity);
            if (!result.Succeeded || result.Result is null)
            {
                return AppResult<FlightsEntity>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            var createdFlight = result.Result;
            return AppResult<FlightsEntity>.CreateSucceeded(new FlightsEntity
            {
                FlightCode = createdFlight.FlightCode,
                AirportID  = createdFlight.AirportID,
                PlaneID    = createdFlight.PlaneID,
            }, "Successfully created flight data");
        }
        catch (Exception ex)
        {
            return AppResult<FlightsEntity>.CreateFailed(ex, "An error occured in creating flight data");
        }
    }

    public async Task<AppResult<FlightsEntity>> DeleteAsync(int Id)
    {
        try
        {
            var result = await dataStore.Flight.GetByIdAsync(Id);

            if (!result.Succeeded || result.Result is null)
            {
                return AppResult<FlightsEntity>.CreateFailed(new ApplicationException("Flight not found"), "Flight with the given ID does not exist.");
            }

            var flightToDelete = result.Result;

            var deleteResult = await dataStore.Flight.Remove(flightToDelete);

            if (!deleteResult.Succeeded)
            {
                return AppResult<FlightsEntity>.CreateFailed(new ApplicationException(deleteResult.Message), deleteResult.Message);
            }

            return AppResult<FlightsEntity>.CreateSucceeded(null, "Successfully deleted flight data.");
        }
        catch (Exception ex)
        {
            return AppResult<FlightsEntity>.CreateFailed(ex, "An error occurred while deleting the flight data.");
        }
    }

    public async Task<AppResult<IEnumerable<FlightsEntity>>> GetAllAsAsync()
    {
        try
        {
            //Options if you want to add palane and airport details
            bool includePlane = true;
            bool includeAirport = true;
            bool includePassenger = true;
          
            var includes = new List<Expression<Func<FlightsEntity, object>>>();
            if (includePlane) includes.Add(a => a.Plane);
            if (includeAirport) includes.Add(a => a.Airport);
            if (includePassenger) includes.Add(a => a.Passengers);

            Expression<Func<FlightsEntity, bool>> filter = a => a.AirportID != null && a.PlaneID != null;

            //You can add skip and take
            var result = await dataStore.Flight.FindAsync(filter,null ,null,includes);

           
            if (!result.Succeeded || result.Result is null || !result.Result.Any())
            {
                return AppResult<IEnumerable<FlightsEntity>>.CreateFailed(new ApplicationException("No flights found."), "No flight records available.");
            }

            
            return AppResult<IEnumerable<FlightsEntity>>.CreateSucceeded(result.Result, "Successfully retrieved flight data.");
        }
        catch (Exception ex)
        {
            // Return failure result in case of an exception
            return AppResult<IEnumerable<FlightsEntity>>.CreateFailed(ex, "An error occurred while retrieving flight data.");
        }
    }

    public async Task<AppResult<FlightsEntity>> UpdateAsync(FlightsEntity flightsEntity)
    {
        try
        {
            var result = await dataStore.Flight.GetByIdAsync(flightsEntity.FlightID);

            if (!result.Succeeded || result.Result is null)
            {
                return AppResult<FlightsEntity>.CreateFailed(new ApplicationException("Flight not found"), "Flight with the given ID does not exist.");
            }

            var existingFlight = result.Result;

            existingFlight.FlightCode = flightsEntity.FlightCode;
            existingFlight.AirportID = flightsEntity.AirportID;
            existingFlight.PlaneID = flightsEntity.PlaneID;
            existingFlight.ChangedOn = DateTime.UtcNow; 
            var updateResult = await dataStore.Flight.Update(existingFlight);

            if (!updateResult.Succeeded)
            {
                return AppResult<FlightsEntity>.CreateFailed(new ApplicationException(updateResult.Message), updateResult.Message);
            }

            return AppResult<FlightsEntity>.CreateSucceeded(existingFlight, "Successfully updated flight data.");
        }
        catch (Exception ex)
        {
            return AppResult<FlightsEntity>.CreateFailed(ex, "An error occurred while updating the flight data.");
        }
    }

}
