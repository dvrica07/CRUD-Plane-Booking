using UMS_PlaneBooking.Repository.Interfaces;
using UMS_PlaneBooking.Repository.DbSets;
using Microsoft.EntityFrameworkCore;

namespace UMS_PlaneBooking.Repository;
public class DataStore : IDataStore
{
    private readonly ApplicationContext applicationContext;
    public DataStore(ApplicationContext applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public IAirport Airport => new AirportEntitySet(applicationContext);

    public IFlight Flight => new FlightEntitySet(applicationContext);

    public IPassenger Passenger => new PassengerEntitySet(applicationContext);

    public IPlane Plane => new PlaneEntitySet(applicationContext);

    public async Task EnsureMigrate()
    {
        await applicationContext.Database.MigrateAsync();
    }

    public async Task SeedData()
    {
        var airportData = await applicationContext.Airports
            .FirstOrDefaultAsync(a => a.AirportName == "NAIA");
        if (airportData == null)
        {
            airportData = new Entities.AirportEntity
            {
                AirportName = "NAIA",
                Address = "Pasay",
                ChangedOn = DateTime.UtcNow,
                CreatedOn = DateTime.UtcNow,
            };
            applicationContext.Airports.Add(airportData);
            await applicationContext.SaveChangesAsync();  // Save to get AirportId
        }

        var planeData = await applicationContext.Planes
            .FirstOrDefaultAsync(p => p.PlaneCode == "BPAL123");
        if (planeData == null)
        {
            planeData = new Entities.PlaneEntity
            {
                PlaneCode = "BPAL123",
                Airline = "PAL",
                Model = "Boeing 737"
            };
            applicationContext.Planes.Add(planeData);
            await applicationContext.SaveChangesAsync();  // Save to get PlaneId
        }

        // Ensure the airport and plane exist
        airportData = await applicationContext.Airports
            .FirstOrDefaultAsync(a => a.AirportName == "NAIA");

        planeData = await applicationContext.Planes
            .FirstOrDefaultAsync(p => p.PlaneCode == "BPAL123");

        if (airportData != null && planeData != null)
        {
            var flightsData = await applicationContext.Flights
                .FirstOrDefaultAsync(f => f.FlightCode == "YU2387f");

            if (flightsData == null)
            {
                flightsData = new Entities.FlightsEntity
                {
                    FlightCode = "YU2387f",
                    AirportID = airportData.AirportID,  // Use the AirportId directly
                    PlaneID = planeData.PlaneID         // Use the PlaneId directly
                };
                applicationContext.Flights.Add(flightsData);
                await applicationContext.SaveChangesAsync();
            }

            // Re-query to ensure flight exists
            flightsData = await applicationContext.Flights
                .FirstOrDefaultAsync(f => f.FlightCode == "YU2387f");

            var passengerData = await applicationContext.Passengers
                .FirstOrDefaultAsync(p => p.PassengerName == "Juan Dela Cruz" && p.FlightID == flightsData.FlightID);
            if (passengerData == null && flightsData != null)
            {
                passengerData = new Entities.PassengerEntity
                {
                    PassengerName = "Juan Dela Cruz",
                    FlightID = flightsData.FlightID  
                };
                applicationContext.Passengers.Add(passengerData);
                await applicationContext.SaveChangesAsync();
            }
        }
    }

}
