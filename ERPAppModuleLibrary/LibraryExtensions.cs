using ERPAppModuleLibrary.Trucks.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ERPAppModuleLibrary;

public static class LibraryExtensions
{
    public static IServiceCollection AddLibraryDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ITrucksService, TrucksService>();

        return serviceCollection;
    }
}