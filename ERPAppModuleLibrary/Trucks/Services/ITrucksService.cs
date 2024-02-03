using ERPAppModuleLibrary.Trucks.PublicModels;

namespace ERPAppModuleLibrary.Trucks.Services;

public interface ITrucksService
{
    Task<TruckResponse> Create(CreateTruckRequest request);
}