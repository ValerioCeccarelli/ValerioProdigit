using System.Net;
using HashidsNet;
using Microsoft.EntityFrameworkCore;
using ValerioProdigit.Api.Data;
using ValerioProdigit.Api.Dtos.Classroom;
using ValerioProdigit.Api.Swagger;

namespace ValerioProdigit.Api.Endpoints.ClassroomEndpoints;

public class GetEndpoint : IEndpointsMapper
{
	public void MapEndpoints(WebApplication app)
	{
		app.MapGet("/Classroom/Get/{classroomId}", GetByCode)
			.WithTags("Classroom")
			.WithDocumentation("Get classroom information","Get classroom information ")
			.WithResponseDocumentation<GetClassroomResponse>(HttpStatusCode.OK, "Classroom information")
			.WithResponseDocumentation<GetClassroomResponse>(HttpStatusCode.BadRequest, "Invalid classroom id")
			.WithResponseDocumentation<GetClassroomResponse>(HttpStatusCode.NotFound, "There is no classroom with the specified code");
	}

	private static async Task<IResult> GetByCode(
		string classroomId, 
		AppDbContext dbContext,
		IHashids hashids)
	{
		if (!hashids.TryDecodeSingle(classroomId, out var id))
		{
			return Results.BadRequest(new GetClassroomResponse()
			{
				Error = "Invalid classroom id"
			});
		}

		var classroom = await dbContext.Classrooms
			.AsNoTracking()
			.Include(x => x.Rows)
			.Where(c => c.Id == id)
			.FirstOrDefaultAsync();

		if (classroom is null)
		{
			return Results.NotFound(new GetClassroomResponse()
			{
				Error = "Building not found"
			});
		}
        
		return Results.Ok(new GetClassroomResponse()
		{
			BuildingId = hashids.Encode(classroom.BuildingId),
			Code = classroom.Code,
			Rows = classroom.Rows.Select(r => r.Seats),
			Capacity = classroom.Capacity
		});
	}
}