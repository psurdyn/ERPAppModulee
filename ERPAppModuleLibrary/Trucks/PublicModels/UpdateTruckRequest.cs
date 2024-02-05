namespace ERPAppModuleLibrary.Trucks.PublicModels;

public record UpdateTruckRequest(string Name, string StatusId, string? NewCode, string? Description = null);