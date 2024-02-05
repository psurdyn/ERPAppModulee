namespace ERPAppModuleLibrary.Trucks.PublicModels;

public record UpdateTruckRequest(string NewCode, string Name, int StatusId, string? Description);