using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UMS_PlaneBooking.Repository;

public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext(string[] args)
    {
        var opts = new DbContextOptionsBuilder<ApplicationContext>();
        opts.UseSqlServer("Server=localhost;Database=UMSDB;Integrated Security=True;");
        return new ApplicationContext(opts.Options);
    }
}
