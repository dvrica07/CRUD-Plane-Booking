using Microsoft.EntityFrameworkCore;
using UMS_PlaneBooking.Repository;
using UMS_PlaneBooking.Repository.Interfaces;
using UMS_PlaneBooking.Services.Repository.Airport;
using UMS_PlaneBooking.Services.Repository.Flight;
using UMS_PlaneBooking.Services.Repository.Interfaces;
using UMS_PlaneBooking.Services.Repository.Passenger;
using UMS_PlaneBooking.Services.Repository.Plane;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDataStore, DataStore>();
builder.Services.AddScoped<IAirportRepository, AirportRepository>();
builder.Services.AddScoped<IPlaneRepository, PlaneRepository>();
builder.Services.AddScoped<IFlightRepository,FlightRepository>();
builder.Services.AddScoped<IPassengerRepository, PassengerRepository>();


builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// seed database data and ensure table are created
using (var scope = app.Services.CreateScope())
{
    var store = scope.ServiceProvider.GetRequiredService<IDataStore>();
    await store.EnsureMigrate();
    await store.SeedData();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
