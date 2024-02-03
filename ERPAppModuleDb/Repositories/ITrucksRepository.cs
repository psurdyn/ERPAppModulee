using ERPAppModuleCommon.Result;
using ERPAppModuleDb.Entities;

namespace ERPAppModuleDb.Repositories;

public interface ITrucksRepository
{
    Task<Result<TrucksEntity>> GetByCode(string code);
    Task<Result<TrucksEntity>> Create(string code, string name, string statusName);
    Task<Result<TrucksEntity>> Update(int id, string code, string name, string statusName);
    Task<Result> Delete(int id);
}