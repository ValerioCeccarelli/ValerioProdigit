namespace ValerioProdigit.Api.Dtos.Lesson;

public sealed class LessonDto
{
	public string Name { get; set; } = default!;
	public string Description { get; set; } = default!;
	
	public string TeacherEmail { get; set; } = default!;
		
	public string BuildingCode { get; set; } = default!;
	public string ClassroomCode { get; set; } = default!;
		
	public string Date { get; set; } = default!;
	public int StartHour { get; set; }
	public int FinishHour { get; set; }

	public string HashId { get; set; } = default!;
}