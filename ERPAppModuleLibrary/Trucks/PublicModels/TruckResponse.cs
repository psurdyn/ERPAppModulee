namespace ERPAppModuleLibrary.Trucks.PublicModels;

public class TruckResponse
{
    public TruckResponse(string code, string name, string status, string? description)
    {
        Code = code;
        Name = name;
        Status = status;
        Description = description;
    }

    public string Code { get; set; }
    public string Name { get; set; }
    public string Status{ get; set; }
    public string? Description { get; set; }
}