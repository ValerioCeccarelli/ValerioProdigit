namespace ValerioProdigit.Api.Dtos.Lesson;

public sealed class GetByTeacherRequest
{
	public string Date { get; set; } = "";
	public string TeacherEmail { get; set; } = "";
}