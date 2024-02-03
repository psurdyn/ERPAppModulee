using ERPAppModuleLibrary.Trucks.PublicModels;
using ERPAppModuleLibrary.Trucks.Services;
using Microsoft.AspNetCore.Mvc;

namespace ERPAppModule.Controllers;

public class TrucksController : ApiController
{
    private readonly ITrucksService _trucksService;
    
    public TrucksController(ITrucksService trucksService)
    {
        _trucksService = trucksService;
    }

    [HttpPut]
    public async Task<ActionResult<TruckResponse>> Create([FromBody] CreateTruckRequest request)
    {
        var result = await _trucksService.Create(request);

        if (result.IsSuccess)
        {
            return result.Response!;
        }

        return result.ExceptionResult.StatusCode switch
        {
            400 => new BadRequestObjectResult(result.ExceptionResult.Exception),
            404 => new NotFoundObjectResult(result.ExceptionResult.Exception),
            _ => throw new Exception("Uknown error")
        };
    }
}