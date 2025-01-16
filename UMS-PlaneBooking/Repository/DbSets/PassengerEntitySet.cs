using UMS_PlaneBooking.Repository.Entities;
using UMS_PlaneBooking.Repository.Interfaces;

namespace UMS_PlaneBooking.Repository.DbSets;

public class PassengerEntitySet : GenericEntity<PassengerEntity>, IPassenger
{
    private readonly ApplicationContext applicationContext;
    public PassengerEntitySet(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }
}
