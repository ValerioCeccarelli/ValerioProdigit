using Swashbuckle.AspNetCore.Filters;
using ValerioProdigit.Api.Dtos.Lesson;

namespace ValerioProdigit.Api.Swagger.ExampleProviders.LessonExamples;

public sealed class AddLessonRequestExample : IExamplesProvider<AddLessonRequest>
{
	public AddLessonRequest GetExamples()
	{
		return new AddLessonRequest()
		{
			BuildingCode = "RM201",
			ClassroomCode = "204",
			Date = DateTime.UtcNow.ToString("yyyy-MM-dd"),
			StartHour = 8,
			FinishHour = 11,
			Name = "Math",
			Description = "Math lesson with the best teacher in the world",
		};
	}
}