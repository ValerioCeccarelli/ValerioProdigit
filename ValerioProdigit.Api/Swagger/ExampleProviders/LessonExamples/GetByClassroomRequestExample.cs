using Swashbuckle.AspNetCore.Filters;
using ValerioProdigit.Api.Dtos.Lesson;

namespace ValerioProdigit.Api.Swagger.ExampleProviders.LessonExamples;

public sealed class GetByClassroomRequestExample : IExamplesProvider<GetByClassroomRequest>
{
	public GetByClassroomRequest GetExamples()
	{
		return new GetByClassroomRequest()
		{
			BuildingCode = "RM201",
			ClassroomCode = "204",
			Date = DateTime.UtcNow.ToString("yyyy-MM-dd")
		};
	}
}