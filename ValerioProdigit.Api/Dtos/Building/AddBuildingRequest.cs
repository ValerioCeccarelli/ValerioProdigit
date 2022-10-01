namespace ValerioProdigit.Api.Dtos.Building;

public sealed class AddBuildingRequest
{
    public string Name { get; set; } = "";
    public string Code { get; set; } = "";
    public string Address { get; set; } = "";
}