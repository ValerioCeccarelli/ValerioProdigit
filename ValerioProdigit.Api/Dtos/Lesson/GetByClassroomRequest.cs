namespace ValerioProdigit.Api.Dtos.Lesson;

public sealed class GetByClassroomRequest
{
	public string Date { get; set; } = "";
	public string ClassroomCode { get; set; } = "";
	public string BuildingCode { get; set; } = "";
}