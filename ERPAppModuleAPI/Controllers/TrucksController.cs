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

    // [HttpPut]
    // public async Task<TruckResponse> Create([FromBody] CreateTruckRequest request)
    // {
    //     
    // }
}