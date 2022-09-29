namespace ValerioProdigit.Api.Dtos.Classroom;

public class GetClassroomResponse
{
	public bool Success => Error.Length == 0;
	public string Error { get; set; } = "";

	public string BuildingId { get; set; }
	public string Code { get; set; }
	public IEnumerable<int> Rows { get; set; }
	public int Capacity { get; set; }
}