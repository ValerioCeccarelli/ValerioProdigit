namespace ValerioProdigit.Api.Dtos.Lesson;

public sealed class AddLessonRequest
{
	public string BuildingCode { get; set; } = "";
	public string ClassroomCode { get; set; } = "";
	public string Date { get; set; } = "";
	
	public int StartHour { get; set; }
	public int FinishHour { get; set; }

	public string Name { get; set; } = "";
	public string Description { get; set; } = "";
}