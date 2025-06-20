using DataBaseContextLibrary;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore; 


namespace StationeryAndSuppliesWebApp.Services; 

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services)
    {
        services.AddScoped<IProductInformationService, ProductInformationService>();

        return services;
    }

    public static IServiceCollection AddAccountService(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();

        return services; 
    }
}
