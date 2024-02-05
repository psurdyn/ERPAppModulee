using ERPAppModuleLibrary.Trucks.PublicModels;
using ERPAppModuleLibrary.Trucks.Services;
using Microsoft.AspNetCore.Mvc;

namespace ERPAppModule.Controllers;

[ApiController]
[Route("[controller]")]
public class TrucksController : ApiController
{
    private readonly ITrucksService _trucksService;
    
    public TrucksController(ITrucksService trucksService)
    {
        _trucksService = trucksService;
    }

    [HttpGet("code")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TruckResponse>> Get([FromRoute] string code)
    {
        var result = await _trucksService.GetByCode(code);
        
        if (result.IsSuccess)
        {
            return result.Response!;
        }

        return result.ExceptionResult.StatusCode switch
        {
            404 => new NotFoundObjectResult(result.ExceptionResult.Exception),
            _ => throw new Exception("Unexpected error occured")
        };
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
            _ => throw new Exception("Unexpected error occured")
        };
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TruckResponse>> Update([FromRoute] string code, [FromBody] UpdateTruckRequest request)
    {
        var result = await _trucksService.Update(code, request);
        
        if (result.IsSuccess)
        {
            return result.Response!;
        }

        return result.ExceptionResult.StatusCode switch
        {
            400 => new BadRequestObjectResult(result.ExceptionResult.Exception),
            404 => new NotFoundObjectResult(result.ExceptionResult.Exception),
            _ => throw new Exception("Unexpected error occured")
        };
    }

    [HttpDelete("code")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] string code)
    {
        var result = await _trucksService.Delete(code);
        
        if (result.IsSuccess)
        {
            return new OkResult();
        }

        return result.ExceptionResult!.StatusCode switch
        {
            400 => new BadRequestObjectResult(result.ExceptionResult.Exception),
            404 => new NotFoundObjectResult(result.ExceptionResult.Exception),
            _ => throw new Exception("Unexpected error occured")
        };
    }
}