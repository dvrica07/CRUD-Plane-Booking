using UMS_PlaneBooking.Repository.Entities;
using UMS_PlaneBooking.Repository.Interfaces;

namespace UMS_PlaneBooking.Repository.DbSets;

public class PlaneEntitySet : GenericEntity<PlaneEntity>, IPlane    
{
    private readonly ApplicationContext applicationContext;
    public PlaneEntitySet(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }
}
