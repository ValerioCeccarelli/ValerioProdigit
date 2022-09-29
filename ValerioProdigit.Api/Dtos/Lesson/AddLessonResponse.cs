namespace ValerioProdigit.Api.Dtos.Lesson;

public class AddLessonResponse
{
	public bool Success => Error.Length == 0;
	public string Error { get; set; } = "";
}