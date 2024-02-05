namespace ERPAppModuleLibrary.Trucks.PublicModels;

public record UpdateTruckRequest(string NewCode, string Name, string StatusId, string? Description);