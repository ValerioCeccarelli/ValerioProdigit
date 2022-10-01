namespace ValerioProdigit.Api.Dtos.Lesson;

public sealed class DeleteLessonResponse
{
	public bool Success => Error.Length == 0;
	public string Error { get; set; } = "";
}