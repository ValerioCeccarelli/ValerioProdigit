namespace ValerioProdigit.Api.Dtos.Building;

public sealed class GetBuildingResponse
{
    public bool Success => Error.Length == 0;
    public string Error { get; set; } = "";
    
    public string Name { get; set; } = "";
    public string Code { get; set; } = "";
    public string Address { get; set; } = "";

    public IEnumerable<string> ClassroomCodes { get; set; } = Enumerable.Empty<string>();
}