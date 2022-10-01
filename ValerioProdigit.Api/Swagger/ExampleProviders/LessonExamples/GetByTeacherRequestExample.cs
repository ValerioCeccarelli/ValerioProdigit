using Swashbuckle.AspNetCore.Filters;
using ValerioProdigit.Api.Dtos.Lesson;

namespace ValerioProdigit.Api.Swagger.ExampleProviders.LessonExamples;

public sealed class GetByTeacherRequestExample : IExamplesProvider<GetByTeacherRequest>
{
	public GetByTeacherRequest GetExamples()
	{
		return new GetByTeacherRequest()
		{
			TeacherEmail = "best.prof@teacher.com",
			Date = DateTime.UtcNow.ToString("yyyy-MM-dd"),
		};
	}
}