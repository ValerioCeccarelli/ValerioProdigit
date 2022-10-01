namespace ValerioProdigit.Api.Dtos.Classroom;

public sealed class GetClassroomResponse
{
	public bool Success => Error.Length == 0;
	public string Error { get; set; } = "";

	public string BuildingId { get; set; } = default!;
	public string Code { get; set; } = default!;
	public IEnumerable<int> Rows { get; set; } = default!;
	public int Capacity { get; set; }
}