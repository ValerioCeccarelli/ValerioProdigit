using Swashbuckle.AspNetCore.Filters;
using ValerioProdigit.Api.Dtos.Building;

namespace ValerioProdigit.Api.Swagger.ExampleProviders.BuildingExamples;

public sealed class AddBuildingRequestExample : IExamplesProvider<AddBuildingRequest>
{
	public AddBuildingRequest GetExamples()
	{
		return new AddBuildingRequest()
		{
			Code = "RM201",
			Name = "Marco Polo",
			Address = "Via Scalo di san Lorenzo"
		};
	}
}