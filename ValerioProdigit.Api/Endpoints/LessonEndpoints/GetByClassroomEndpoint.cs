using System.Net;
using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ValerioProdigit.Api.Data;
using ValerioProdigit.Api.Dtos.Lesson;
using ValerioProdigit.Api.Swagger;
using ValerioProdigit.Api.Validators;

namespace ValerioProdigit.Api.Endpoints.LessonEndpoints;

public class GetByClassroomEndpoint : IEndpointsMapper
{
	public void MapEndpoints(WebApplication app)
	{
		app.MapPost("/Lesson/GetByClassroom", GetByClassroom)
			.WithTags("Lesson")
			.WithDocumentation("Get all Classroom's Lessons", "Get the list of all Lesson for a specific Classroom in a given day")
			.WithResponseDocumentation<GetByClassroomResponse>(HttpStatusCode.OK, "List of lessons")
			.WithResponseDocumentation<GetByClassroomResponse>(HttpStatusCode.BadRequest, "Some of the provided data is invalid")
			.WithResponseDocumentation<GetByClassroomResponse>(HttpStatusCode.NotFound, "Building or Classroom not found");
	}

	private static async Task<IResult> GetByClassroom(
		[FromBody] GetByClassroomRequest request,
		AppDbContext context,
		IValidator<GetByClassroomRequest> validator,
		IHashids hashids)
	{
		var validationResult = validator.Validate(request);
		if (!validationResult.Succeeded)
		{
			return Results.BadRequest(new GetByClassroomResponse()
			{
				Error = validationResult.Error
			});
		}
		
		var building  = await context.Buildings
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Code == request.BuildingCode);
		if (building is null)
		{
			return Results.NotFound(new AddLessonResponse()
			{
				Error = "Building not found"
			});
		}
		
		var classroom = await context.Classrooms
			.AsNoTracking()
			.Where(x => x.BuildingId == building.Id)
			.FirstOrDefaultAsync(x => x.Code == request.ClassroomCode);
		if (classroom is null)
		{
			return Results.NotFound(new AddLessonResponse()
			{
				Error = "Classroom not found"
			});
		}
		
		var lessons = await context.Lessons
			.AsNoTracking()
			.Where(x => x.ClassroomId == classroom.Id)
			.Where(x => x.Date == DateTime.Parse(request.Date))
			.Include(x => x.Teacher)
			.ToListAsync();

		return Results.Ok(new GetByClassroomResponse()
		{
			Lessons = lessons.Select(x => new LessonDto()
			{
				BuildingCode = building.Code,
				ClassroomCode = classroom.Code,
				
				Date = x.Date.ToString("yyyy-MM-dd"),
				StartHour = x.StartHour,
				FinishHour = x.FinishHour,
				
				TeacherEmail = x.Teacher.Email,
				
				Name = x.Name,
				Description = x.Description,
				
				HashId = hashids.Encode(x.Id)
			})
		});
	}
}