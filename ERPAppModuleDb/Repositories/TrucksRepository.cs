using ERPAppModuleCommon.Exceptions;
using ERPAppModuleCommon.Result;
using ERPAppModuleDb.Data;
using ERPAppModuleDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERPAppModuleDb.Repositories;

public class TrucksRepository : ITrucksRepository
{
    private readonly DataContext _context;

    public TrucksRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Result<TrucksEntity>> GetByCode(string code)
    {
        var truck = await _context.Set<TrucksEntity>().Include(x => x.Status).SingleOrDefaultAsync(x => x.Code == code);
        if (truck == null)
        {
            return Result<TrucksEntity>.Failure(new NoTruckForProvidedCodeException(code), 404);
        }

        return Result<TrucksEntity>.Success(truck);
    }

    public async Task<Result<TrucksEntity>> Create(string code, string name, string statusName)
    {
        var status = await GetStatus(statusName);
        if (status.IsFailure)
        {
            return Result<TrucksEntity>.Failure(status.ExceptionResult.Exception, status.ExceptionResult.StatusCode);
        }

        await CheckCodeUniqueness(code);
        
        var newEntity = new TrucksEntity
        {
            Code = code, Name = name, Status = status.Response!
        };

        _context.Add(newEntity);
        await _context.SaveChangesAsync();

        var createdEntity = await _context.Set<TrucksEntity>().Include(x => x.Status).FirstAsync(x => x.Code == code);
        return Result<TrucksEntity>.Success(createdEntity);
    }

    public async Task<Result<TrucksEntity>> Update(int id, string code, string name, string statusName)
    {
        var truckEntity = await _context.Set<TrucksEntity>().FirstOrDefaultAsync(x => x.Id == id);
        if (truckEntity == null)
        {
            return Result<TrucksEntity>.Failure(new InvalidTruckIdException(id), 404);
        }
        
        var status = await GetStatus(statusName);
        if (status.IsFailure)
        {
            return Result<TrucksEntity>.Failure(status.ExceptionResult.Exception, status.ExceptionResult.StatusCode);
        }

        truckEntity.Code = code;
        truckEntity.Name = name;
        truckEntity.Status = status.Response!;

        await _context.SaveChangesAsync();

        return Result<TrucksEntity>.Success(truckEntity);
    }

    public async Task<Result> Delete(int id)
    {
        var truckEntity = await _context.Set<TrucksEntity>().FirstOrDefaultAsync(x => x.Id == id);
        if (truckEntity == null)
        {
            return Result.Failure(new InvalidTruckIdException(id), 404);
        }

        _context.Set<TrucksEntity>().Remove(truckEntity);

        await _context.SaveChangesAsync();

        return Result.Success();
    }

    private async ValueTask<Result> CheckCodeUniqueness(string code)
    {
        if (await _context.Set<TrucksEntity>().AnyAsync(x => x.Code == code))
        {
            return Result.Failure(new CodeAlreadyUsedException(code), 400);
        }

        return Result.Success();
    }

    private async Task<Result<TruckStatusesDictionary>> GetStatus(string statusName)
    {
        var status = await _context.Set<TruckStatusesDictionary>()
            .FirstOrDefaultAsync(x => x.Name.ToLowerInvariant() == statusName.ToLowerInvariant());
        if (status == null)
        {
            return Result<TruckStatusesDictionary>.Failure(new InvalidStatusException(statusName), 400);
        }

        return Result<TruckStatusesDictionary>.Success(status);
    }
}