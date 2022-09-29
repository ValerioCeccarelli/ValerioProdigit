namespace ValerioProdigit.Api.Dtos.Lesson;

public class GetLessonResponse
{
	public bool Success => Error.Length == 0;
	public string Error { get; set; } = "";
	
	public string Name { get; set; } = default!;
	public string Description { get; set; } = default!;
	public string BuildingCode { get; set; } = default!;
	public string ClassroomCode { get; set; } = default!;
	public int StartHour { get; set; }
	public int FinishHour { get; set; }
	public string HashId { get; set; } = default!;
	public string TeacherEmail { get; set; } = default!;
}