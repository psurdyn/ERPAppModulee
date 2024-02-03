namespace ERPAppModuleLibrary.Trucks.PublicModels;

public record CreateTruckRequest(string Code, string Name, TruckStatus Status, string? Description);