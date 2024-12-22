using System.Data;
using CarRentPlace.BLL.Services.Implementations;
using CarRentPlace.BLL.Services.Interfaces;
using CarRentPlace.DAL.Repositories.Implementations;
using CarRentPlace.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Npgsql;

namespace CarRentPlace.Presentation.Extensions;

public static class WebApplicationBuilderExtension
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllersWithViews();

        builder.Services.AddScoped<IBookingRepository, BookingRepository>();
        builder.Services.AddScoped<ICarBodyTypeRepository, CarBodyTypeRepository>();
        builder.Services.AddScoped<ICarBrandRepository, CarBrandRepository>();
        builder.Services.AddScoped<ICarFeatureRepository, CarFeatureRepository>();
        builder.Services.AddScoped<ICarModelRepository, CarModelRepository>();
        builder.Services.AddScoped<ICarRepository, CarRepository>();
        builder.Services.AddScoped<IDriveTypeRepository, DriveTypeRepository>();
        builder.Services.AddScoped<IFeatureRepository, FeatureRepository>();
        builder.Services.AddScoped<ILogRepository, LogRepository>();
        builder.Services.AddScoped<IRentOfferImageRepository, RentOfferImageRepository>();
        builder.Services.AddScoped<IRentOfferRepository, RentOfferRepository>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        builder.Services.AddScoped<ITransmissionTypeRepository, TransmissionTypeRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        
        builder.Services.AddScoped<IUserService, UserService>();

        return builder;
    }
    
    public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "Authentication";
                options.LoginPath = "/Account/Authorization";
                options.Cookie.HttpOnly = true;
                options.SlidingExpiration = true;
            });

        return builder;
    }
    
    public static WebApplicationBuilder AddDataBase(this WebApplicationBuilder builder)
    {
        string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddScoped<IDbConnection>(sp => new NpgsqlConnection(connectionString));

        return builder;
    }
}