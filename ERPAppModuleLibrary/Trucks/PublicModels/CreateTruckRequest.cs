namespace ERPAppModuleLibrary.Trucks.PublicModels;

public record CreateTruckRequest(string Code, string Name, int statusId, string? Description);