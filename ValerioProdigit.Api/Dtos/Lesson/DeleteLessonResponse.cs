namespace ValerioProdigit.Api.Dtos.Lesson;

public class DeleteLessonResponse
{
	public bool Success => Error.Length == 0;
	public string Error { get; set; } = "";
}