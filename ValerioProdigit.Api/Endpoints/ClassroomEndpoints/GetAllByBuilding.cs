using HashidsNet;

namespace ValerioProdigit.Api.Endpoints.ClassroomEndpoints;

public class GetAllByBuilding : IEndpointsMapper
{
	public void MapEndpoints(WebApplication app)
	{
		//todo: add getall endpoint
	}

	private static async Task<IResult> GetAll(string buildingId, IHashids hashids)
	{
		return Results.Ok();
	}
}