using UMS_PlaneBooking.Repository.Entities;
using UMS_PlaneBooking.Repository.Interfaces;

namespace UMS_PlaneBooking.Repository.DbSets;

public class FlightEntitySet : GenericEntity<FlightsEntity>, IFlight
{
    private readonly ApplicationContext applicationContext;
    public FlightEntitySet(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }
}
