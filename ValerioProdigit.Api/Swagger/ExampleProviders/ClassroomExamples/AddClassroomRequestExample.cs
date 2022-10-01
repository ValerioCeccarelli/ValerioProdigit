using Swashbuckle.AspNetCore.Filters;
using ValerioProdigit.Api.Dtos.Classroom;

namespace ValerioProdigit.Api.Swagger.ExampleProviders.ClassroomExamples;

public sealed class AddClassroomRequestExample : IExamplesProvider<AddClassroomRequest>
{
	public AddClassroomRequest GetExamples()
	{
		return new AddClassroomRequest()
		{
			BuildingCode = "RM201",
			ClassroomCode = "204",
			Seats = 50,
			Rows = new[] {20, 20, 10}
		};
	}
}