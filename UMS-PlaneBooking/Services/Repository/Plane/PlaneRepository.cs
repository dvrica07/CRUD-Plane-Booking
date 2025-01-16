using UMS_PlaneBooking.Models.Common;
using UMS_PlaneBooking.Repository.Entities;
using UMS_PlaneBooking.Repository.Interfaces;
using UMS_PlaneBooking.Services.Repository.Interfaces;

namespace UMS_PlaneBooking.Services.Repository.Plane;

public class PlaneRepository : IPlaneRepository
{
    private readonly IDataStore dataStore;
    public PlaneRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public async Task<AppResult<PlaneEntity>> CreateAsync(PlaneEntity planeEntity)
    {
        try
        {
            var result = await dataStore.Plane.Add(planeEntity);
            if (!result.Succeeded || result.Result is null)
            {
                return AppResult<PlaneEntity>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            var createdplane = result.Result;
            return AppResult<PlaneEntity>.CreateSucceeded(new PlaneEntity
            {
                Airline   = createdplane.Airline,
                Model     = createdplane.Model,
                Flights   = createdplane.Flights,
                PlaneCode = createdplane.PlaneCode,
                PlaneID   = createdplane.PlaneID
            }, "Successfully created plane data");
        }
        catch (Exception ex)
        {
            return AppResult<PlaneEntity>.CreateFailed(ex, "An error occured in creating plane data");
        }
    }

    public async Task<AppResult<PlaneEntity>> DeleteAsync(int Id)
    {
        try
        {
            // Retrieve the flight entity by ID
            var result = await dataStore.Plane.GetByIdAsync(Id);

            if (!result.Succeeded || result.Result is null)
            {
                return AppResult<PlaneEntity>.CreateFailed(new ApplicationException("Plane not found"), "Plane with the given ID does not exist.");
            }

            var planeToDelete = result.Result;

            var deleteResult = await dataStore.Plane.Remove(planeToDelete);
            if (!deleteResult.Succeeded || deleteResult.Result is null)
            {
                return AppResult<PlaneEntity>.CreateFailed(new ApplicationException(deleteResult.Message), deleteResult.Message);
            }

            return AppResult<PlaneEntity>.CreateSucceeded(null, "Successfully deleted plane data");
        }
        catch (Exception ex)
        {
            return AppResult<PlaneEntity>.CreateFailed(ex, "An error occurred while deleting plane data");
        }
    }

    public async Task<AppResult<IEnumerable<PlaneEntity>>> GetAllAsAsync()
    {
        try
        {
            var result = await dataStore.Plane.GetAllAsync();
            if (!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<PlaneEntity>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<IEnumerable<PlaneEntity>>.CreateSucceeded(result.Result, "Successfully retrieved all planes");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<PlaneEntity>>.CreateFailed(ex, "An error occurred while retrieving planes data");
        }
    }

    public async Task<AppResult<PlaneEntity>> UpdateAsync(PlaneEntity planeEntity)
    {
        try
        {
            var result = await dataStore.Plane.GetByIdAsync(planeEntity.PlaneID);

            if (!result.Succeeded || result.Result is null)
            {
                return AppResult<PlaneEntity>.CreateFailed(new ApplicationException("Plane not found"), "Plane with the given ID does not exist.");
            }

            var existingPlane = result.Result;

            existingPlane.ChangedOn = planeEntity.ChangedOn;

            var updateResult = await dataStore.Plane.Update(existingPlane);
            if (!result.Succeeded || result.Result is null)
            {
                return AppResult<PlaneEntity>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<PlaneEntity>.CreateSucceeded(result.Result, "Successfully updated plane data");
        }
        catch (Exception ex)
        {
            return AppResult<PlaneEntity>.CreateFailed(ex, "An error occurred while updating plane data");
        }
    }
}
