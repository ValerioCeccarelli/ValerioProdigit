namespace ValerioProdigit.Api.Dtos.Classroom;

public class AddClassroomRequest
{
    public string ClassroomCode { get; set; } = "";
    public string BuildingCode { get; set; } = "";

    public int Seats { get; set; }

    public IEnumerable<int> Rows { get; set; } = Array.Empty<int>();
}