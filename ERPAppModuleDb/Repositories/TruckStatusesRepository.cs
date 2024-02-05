using ERPAppModuleCommon.Exceptions;
using ERPAppModuleCommon.Result;
using ERPAppModuleDb.Data;
using ERPAppModuleDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERPAppModuleDb.Repositories;

public class TruckStatusesRepository : ITruckStatusesRepository
{
    private readonly DataContext _context;

    public TruckStatusesRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Result<TruckStatusesDictionary>> GetStatus(string statusId)
    {
        var status = await _context.Set<TruckStatusesDictionary>()
            .FirstOrDefaultAsync(x => x.Id == statusId);
        if (status == null)
        {
            return Result<TruckStatusesDictionary>.Failure(new StatusNotFoundException(statusId), 400);
        }

        return Result<TruckStatusesDictionary>.Success(status);
    }

    public async Task<IList<TruckStatusesDictionary>> GetStatuses(IEnumerable<string> ids)
    {
        return await _context.Set<TruckStatusesDictionary>().Where(x => ids.Contains(x.Id)).ToListAsync();
    }
}