﻿using Microsoft.EntityFrameworkCore;
using UMS_PlaneBooking.Models.Common;
using UMS_PlaneBooking.Repository.Entities;
using UMS_PlaneBooking.Repository.Interfaces;

namespace UMS_PlaneBooking.Repository.DbSets;
public class AirportEntitySet : GenericEntity<AirportEntity>, IAirport
{
    private readonly ApplicationContext applicationContext;
    public AirportEntitySet(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<AirportEntity>>GetAirportName(string Name)
    {
        try
        {
            var result = await applicationContext.Airports.FirstOrDefaultAsync(w => w.AirportName == Name );
            if (result == null)
            {
                return AppResult<AirportEntity>.CreateFailed(new ApplicationException("Can't find airport name"), "Can't find airport name");
            }

            return AppResult<AirportEntity>.CreateSucceeded(result, "Successfully find airport name airport name");
        }
        catch (Exception ex)
        {
            return AppResult<AirportEntity>.CreateFailed(ex, "An error occured while getting airport name");
        }
    }
}
