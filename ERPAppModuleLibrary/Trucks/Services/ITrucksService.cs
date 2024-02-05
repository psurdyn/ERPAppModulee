using ERPAppModuleCommon.Result;
using ERPAppModuleLibrary.Trucks.PublicModels;

namespace ERPAppModuleLibrary.Trucks.Services;

public interface ITrucksService
{
    Task<Result<TruckResponse>> GetByCode(string code);
    Task<Result<TruckResponse>> Create(CreateTruckRequest request);
    Task<Result<TruckResponse>> Update(string code, UpdateTruckRequest request);
    Task<Result> Delete(string code);
    Task<IEnumerable<TruckResponse>> QueryItems(QueryObject queryObject);
}