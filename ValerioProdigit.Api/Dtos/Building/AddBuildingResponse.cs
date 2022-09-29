namespace ValerioProdigit.Api.Dtos.Building;

public class AddBuildingResponse
{
    public bool Success => Error.Length == 0;
    public string Error { get; set; } = "";
}