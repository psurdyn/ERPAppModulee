using ERPAppModuleCommon.Result;
using ERPAppModuleDb.Entities;

namespace ERPAppModuleDb.Repositories;

public interface ITrucksRepository
{
    Task<Result<TrucksEntity>> GetByCode(string code);
    Task<Result<TrucksEntity>> Create(string code, string name, int statusId);

    Task<Result<TrucksEntity>> Update(string code, string name, int statusId, string? newCode, string? description = null);
    Task<Result> Delete(int id);
}