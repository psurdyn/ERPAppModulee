namespace ERPAppModuleLibrary.Trucks.PublicModels;

public class QueryObject
{
    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? StatusId {get;set;}

    public bool IsSortAscending { get; set; }

    public string SortBy { get; set; } = "code";

    public int Page { get; set; } = 1;

    public int? PageSize { get; set; }
}