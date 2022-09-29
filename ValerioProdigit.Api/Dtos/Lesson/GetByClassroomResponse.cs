
namespace ValerioProdigit.Api.Dtos.Lesson;

public class GetByClassroomResponse
{
	public bool Success => Error.Length == 0;
	public string Error { get; set; } = "";

	public IEnumerable<LessonDto> Lessons { get; set; } = Enumerable.Empty<LessonDto>();
}