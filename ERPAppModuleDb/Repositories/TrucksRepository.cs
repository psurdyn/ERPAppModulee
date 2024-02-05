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

    public async Task<Result<TrucksEntity>> Create(string code, string name, int statusId)
    {
        var status = await GetStatus(statusId);
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

    public async Task<Result<TrucksEntity>> Update(string code, string name, int statusId, string? newCode, string? description = null)
    {
        var truckEntity = await _context.Set<TrucksEntity>().FirstOrDefaultAsync(x => x.Code == code);
        if (truckEntity == null)
        {
            return Result<TrucksEntity>.Failure(new NoTruckForProvidedCodeException(code), 404);
        }
        
        var status = await GetStatus(statusId);
        if (status.IsFailure)
        {
            return Result<TrucksEntity>.Failure(status.ExceptionResult.Exception, status.ExceptionResult.StatusCode);
        }
        
        truckEntity.Name = name;
        truckEntity.Status = status.Response!;

        if (newCode != null && newCode != code)
        {
            await CheckCodeUniqueness(newCode);
            truckEntity.Code = newCode;
        }

        if (description != null)
        {
            truckEntity.Description = description;
        }

        await _context.SaveChangesAsync();

        return Result<TrucksEntity>.Success(truckEntity);
    }

    public async Task<Result> Delete(string code)
    {
        var truckEntity = await _context.Set<TrucksEntity>().FirstOrDefaultAsync(x => x.Code == code);
        if (truckEntity == null)
        {
            return Result.Failure(new NoTruckForProvidedCodeException(code), 404);
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

    private async Task<Result<TruckStatusesDictionary>> GetStatus(int statusId)
    {
        var status = await _context.Set<TruckStatusesDictionary>()
            .FirstOrDefaultAsync(x => x.Id == statusId);
        if (status == null)
        {
            return Result<TruckStatusesDictionary>.Failure(new StatusNotFoundException(statusId), 400);
        }

        return Result<TruckStatusesDictionary>.Success(status);
    }
}