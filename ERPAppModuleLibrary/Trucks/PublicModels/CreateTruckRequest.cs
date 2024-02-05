namespace ERPAppModuleLibrary.Trucks.PublicModels;

public record CreateTruckRequest(string Code, string Name, string StatusId, string? Description);