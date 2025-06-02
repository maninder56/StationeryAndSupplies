using DataBaseContextLibrary;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore; 


namespace StationeryAndSuppliesWebApp.Services; 

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services)
    {
        


        return services;
    }
}
