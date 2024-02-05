using ERPAppModuleCommon.Result;
using ERPAppModuleDb.Entities;

namespace ERPAppModuleDb.Repositories;

public interface ITruckStatusesRepository
{
    Task<Result<TruckStatusesDictionary>> GetStatus(string statusId);
    Task<IList<TruckStatusesDictionary>> GetStatuses(IEnumerable<string> ids);
}