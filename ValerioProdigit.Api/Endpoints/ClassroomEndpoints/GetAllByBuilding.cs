using System.Net;
using HashidsNet;
using Microsoft.EntityFrameworkCore;
using ValerioProdigit.Api.Data;
using ValerioProdigit.Api.Dtos.Classroom;
using ValerioProdigit.Api.Swagger;

namespace ValerioProdigit.Api.Endpoints.ClassroomEndpoints;

public class GetAllByBuilding : IEndpointsMapper
{
	public void MapEndpoints(WebApplication app)
	{
		app.MapGet("Classroom/GetAll/{buildingId}", GetAll)
			.WithTags("Classroom")
			.WithDocumentation("Get classrooms list","")
			.WithResponseDocumentation<GetAllByBuildingResponse>(HttpStatusCode.OK, "Classrooms list")
			.WithResponseDocumentation<GetAllByBuildingResponse>(HttpStatusCode.BadRequest, "Invalid building id")
			.WithResponseDocumentation<GetAllByBuildingResponse>(HttpStatusCode.NotFound, "There is no building with this id");

	}

	private static async Task<IResult> GetAll(
		string buildingId, 
		AppDbContext context,
		IHashids hashids)
	{
		if (!hashids.TryDecodeSingle(buildingId, out var id))
		{
			return Results.BadRequest(new GetAllByBuildingResponse()
			{
				Error = "Invalid building Id"
			});
		}

		var building = await context.Buildings
			.Where(x => x.Id == id)
			.Include(b => b.Classrooms)
			.ThenInclude(c => c.Rows)
			.FirstOrDefaultAsync();

		if (building is null)
		{
			return Results.NotFound(new GetAllByBuildingResponse()
			{
				Error = "Building not found"
			});
		}
		
		return Results.Ok(new GetAllByBuildingResponse()
		{
			Classrooms = building.Classrooms.Select(c => new GetAllByBuildingResponse.Classroom()
			{
				Capacity = c.Capacity,
				Code = c.Code,
				Id = hashids.Encode(c.Id),
				Rows = c.Rows.OrderBy(r=>r.Index).Select(r => r.Seats)
			})
		});
	}
}