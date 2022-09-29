
namespace ValerioProdigit.Api.Dtos.Building;

public class GetAllBuildingResponse
{
    public bool Success => Error.Length == 0;
    public string Error { get; set; } = "";
    public IEnumerable<Building> Buildings { get; set; } = Enumerable.Empty<Building>();

    public class Building
    {
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public string Address { get; set; } = "";
    }
}