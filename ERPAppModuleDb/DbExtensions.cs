using ERPAppModuleDb.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ERPAppModuleDb;

public static class DbExtensions
{
    public static IServiceCollection AddDbDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ITrucksRepository, TrucksRepository>();
        serviceCollection.AddScoped<ITruckStatusesRepository, TruckStatusesRepository>();

        return serviceCollection;
    }
}