using System.Linq.Expressions;
using System.Text.RegularExpressions;
using ERPAppModuleCommon;
using ERPAppModuleCommon.Exceptions;
using ERPAppModuleCommon.Result;
using ERPAppModuleDb.Entities;
using ERPAppModuleDb.Repositories;
using ERPAppModuleLibrary.Trucks.PublicModels;

namespace ERPAppModuleLibrary.Trucks.Services;

public class TrucksService : ITrucksService
{
    private readonly ITrucksRepository _trucksRepository;
    private readonly ITruckStatusesRepository _truckStatusesRepository;

    public TrucksService(ITrucksRepository trucksRepository, ITruckStatusesRepository truckStatusesRepository)
    {
        _trucksRepository = trucksRepository;
        _truckStatusesRepository = truckStatusesRepository;
    }

    public async Task<Result<TruckResponse>> GetByCode(string code)
    {
        var truckEntity = await _trucksRepository.GetByCode(code);
        if (truckEntity.IsFailure)
        {
            return Result<TruckResponse>.Failure(truckEntity.ExceptionResult);
        }

        return Result<TruckResponse>.Success(new TruckResponse(truckEntity.Response!.Code, truckEntity.Response.Name,
            truckEntity.Response.Status.Name, ""));
    }

    public async Task<Result<TruckResponse>> Create(CreateTruckRequest request)
    {
        var formatValidationResult = ValidateIfCodeMatchTheFormat(request.Code);
        if (formatValidationResult.IsFailure)
        {
            return Result<TruckResponse>.Failure(formatValidationResult.ExceptionResult!);
        }

        var newEntityResult = await _trucksRepository.Create(request.Code, request.Name, request.StatusId);
        if (newEntityResult.IsFailure)
        {
            return Result<TruckResponse>.Failure(newEntityResult.ExceptionResult);
        }

        var response = new TruckResponse(newEntityResult.Response!.Code, newEntityResult.Response.Name,
            newEntityResult.Response.Status.Name, newEntityResult.Response.Description);
        return Result<TruckResponse>.Success(response);
    }

    public async Task<Result<TruckResponse>> Update(string code, UpdateTruckRequest request)
    {
        var validationResult = await ValidateIfTruckCanBeUpdated(code, request.StatusId, request.NewCode);
        if (validationResult.IsFailure)
        {
            return Result<TruckResponse>.Failure(validationResult.ExceptionResult!);
        }
        
        var updateResult = await _trucksRepository.Update(code, request.Name, request.StatusId, request.NewCode,
            request.Description);
        if (updateResult.IsFailure)
        {
            return Result<TruckResponse>.Failure(updateResult.ExceptionResult);
        }

        var response = new TruckResponse(updateResult.Response!.Code, updateResult.Response.Name,
            updateResult.Response.Status.Name, updateResult.Response.Description);
        return Result<TruckResponse>.Success(response);
    }

    public async Task<Result> Delete(string code)
    {
        var truckEntity = await _trucksRepository.GetByCode(code);
        if (truckEntity.IsFailure)
        {
            return Result.Failure(truckEntity.ExceptionResult);
        }

        var deleteResult = await _trucksRepository.Delete(code);
        if (deleteResult.IsFailure)
        {
            return Result.Failure(deleteResult.ExceptionResult!);
        }

        return Result.Success();
    }

    public async Task<IEnumerable<TruckResponse>> QueryItems(QueryObject queryObject)
    {
        var query = _trucksRepository.GetTrucksQueryable();
        
        if (!string.IsNullOrEmpty(queryObject.StatusId))
        {
            query = query.Where(x => x.StatusId.Contains(queryObject.StatusId));
        }

        if (!string.IsNullOrEmpty(queryObject.Code))
        {
            query = query.Where(x => x.Code.Contains(queryObject.Code));
        }

        if (!string.IsNullOrEmpty(queryObject.Name))
        {
            query = query.Where(x => x.Name.Contains(queryObject.Name));
        }
        
        var columnsMap = new Dictionary<string, Expression<Func<TrucksEntity, object>>>
        {
            ["name"] = x => x.Name,
            ["statusId"] = x => x.StatusId,
            ["code"] = x => x.Code
        };
        query = ApplyOrdering(query, queryObject, columnsMap);

        var pageData = GetPage(query, queryObject.Page, queryObject.PageSize ?? 20);

        return pageData.Select(x => new TruckResponse(x.Code, x.Name, x.Status.Name, x.Description));
    }

    private IEnumerable<TrucksEntity> GetPage(IQueryable<TrucksEntity> query, int page, int pageSize)
    {
        query.TryGetNonEnumeratedCount(out int count);

        if (page == 1 || count > (page - 1) * pageSize)
        {
            return query.Skip((page - 1) * pageSize).Take(pageSize);   
        }

        return Enumerable.Empty<TrucksEntity>();
    }

    private static IQueryable<TrucksEntity> ApplyOrdering(IQueryable<TrucksEntity> query, QueryObject queryObj, Dictionary<string, Expression<Func<TrucksEntity, object>>> columnsMap)
    {
        if (string.IsNullOrWhiteSpace(queryObj.SortBy) || !columnsMap.ContainsKey(queryObj.SortBy))
            return query;

        return queryObj.IsSortAscending ? query.OrderBy(columnsMap[queryObj.SortBy]) : query.OrderByDescending(columnsMap[queryObj.SortBy]);
    }
    
    private async ValueTask<Result> ValidateIfTruckCanBeUpdated(string code, string newStatusId, string? newCode)
    {
        var truckEntity = await _trucksRepository.GetByCode(code);
        if (truckEntity.IsFailure)
        {
            return Result.Failure(truckEntity.ExceptionResult);
        }

        if (newCode != null)
        {
            var validationResult = ValidateIfCodeMatchTheFormat(newCode);
            if(validationResult.IsFailure)
            {
                return Result.Failure(validationResult.ExceptionResult!);
            }   
        }

        if (truckEntity.Response!.StatusId != newStatusId)
        {
            var validateStatusResult = ValidateStatusChange(truckEntity.Response!.StatusId, newStatusId);
            if (validateStatusResult.IsFailure)
            {
                return Result.Failure(validateStatusResult.ExceptionResult!);
            }
        }

        return Result.Success();
    }
    
    private Result ValidateIfCodeMatchTheFormat(string code)
    {
        var regex = new Regex("^[a-zA-Z][a-zA-Z0-9]*$");
        return regex.Match(code).Success ? Result.Success() : Result.Failure(new InvalidCodeFormatException(code), 400);
    }

    private Result ValidateStatusChange(string currentStatus, string newStatus)
    {
        if (currentStatus == nameof(Constants.OutOfServiceStatus) || newStatus == nameof(Constants.OutOfServiceStatus))
        {
            return Result.Success();    
        }

        if (currentStatus == nameof(Constants.LoadingStatus) && newStatus == nameof(Constants.ToJobStatus))
        {
            return Result.Success();
        }

        if (currentStatus == nameof(Constants.ToJobStatus) && newStatus == Constants.AtJobStatus)
        {
            return Result.Success();
        }

        if (currentStatus == Constants.AtJobStatus && newStatus == Constants.ReturningStatus)
        {
            return Result.Success();
        }

        if (currentStatus == Constants.ReturningStatus && newStatus == Constants.LoadingStatus)
        {
            return Result.Success();
        }

        return Result.Failure(new InvalidNewStatusException(newStatus), 400);
    }
}