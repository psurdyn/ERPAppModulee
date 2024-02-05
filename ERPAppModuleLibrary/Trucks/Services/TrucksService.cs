using System.Text.RegularExpressions;
using ERPAppModuleCommon.Exceptions;
using ERPAppModuleCommon.Result;
using ERPAppModuleDb.Repositories;
using ERPAppModuleLibrary.Trucks.PublicModels;

namespace ERPAppModuleLibrary.Trucks.Services;

public class TrucksService : ITrucksService
{
    private readonly ITrucksRepository _trucksRepository;

    public TrucksService(ITrucksRepository trucksRepository)
    {
        _trucksRepository = trucksRepository;
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

        var newEntityResult = await _trucksRepository.Create(request.Code, request.Name, request.statusId);
        if (newEntityResult.IsFailure)
        {
            return Result<TruckResponse>.Failure(newEntityResult.ExceptionResult);
        }

        var response = new TruckResponse(newEntityResult.Response!.Code, newEntityResult.Response.Name,
            newEntityResult.Response.Status.Name, newEntityResult.Response.Status.Name);
        return Result<TruckResponse>.Success(response);
    }

    public async Task<Result<TruckResponse>> Update(string code, UpdateTruckRequest request)
    {
        var truckEntity = await _trucksRepository.GetByCode(code);
        if (truckEntity.IsFailure)
        {
            return Result<TruckResponse>.Failure(truckEntity.ExceptionResult);
        }

        ValidateIfCodeMatchTheFormat(request.NewCode);

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

    private Result ValidateIfCodeMatchTheFormat(string code)
    {
        var regex = new Regex("^[a-zA-Z][a-zA-Z0-9]*$");
        return regex.Match(code).Success ? Result.Success() : Result.Failure(new InvalidCodeFormatException(code), 400);
    }
}