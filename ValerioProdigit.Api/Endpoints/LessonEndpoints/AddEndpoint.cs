using System.Net;
using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ValerioProdigit.Api.Auth;
using ValerioProdigit.Api.Data;
using ValerioProdigit.Api.Dtos.Lesson;
using ValerioProdigit.Api.Models;
using ValerioProdigit.Api.Swagger;
using ValerioProdigit.Api.Validators;

namespace ValerioProdigit.Api.Endpoints.LessonEndpoints;

public sealed class AddEndpoint : IEndpointsMapper
{
	public void MapEndpoints(WebApplication app)
	{
		app.MapPost("/Lesson/Add", Add)
			.WithTags("Lesson")
			.RequireAuthorization(Policy.NonStudent)
			.WithDocumentation("Create new Lesson","Only Admins and Teachers can create new Lessons")
			.WithResponseDocumentation(HttpStatusCode.Unauthorized, "You need a valid token to access this endpoint")
			.WithResponseDocumentation(HttpStatusCode.Forbidden, "You don't have the required permissions to access this endpoint (admin or teacher role required)")
			.WithResponseDocumentation<AddLessonResponse>(HttpStatusCode.Created, "Lesson created successfully")
			.WithResponseDocumentation<AddLessonResponse>(HttpStatusCode.BadRequest, "Some of the provided data is invalid")
			.WithResponseDocumentation<AddLessonResponse>(HttpStatusCode.NotFound, "Building or Classroom not found");
	}

	private static async Task<IResult> Add(
		[FromBody] AddLessonRequest request,
		AppDbContext context,
		IValidator<AddLessonRequest> validator,
		HttpContext httpContext,
		IHashids hashids)
	{
		var validationResult = validator.Validate(request);
		if (!validationResult.Succeeded)
		{
			return Results.BadRequest(new AddLessonResponse()
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

		var date = DateTime.Parse(request.Date);
		var lessons = await context.Lessons
			.AsNoTracking()
			.Where(x => x.ClassroomId == classroom.Id)
			.Where(x => x.Date == date)
			.ToListAsync();

		var newLesson = new Lesson()
		{
			Date = date,
			StartHour = request.StartHour,
			FinishHour = request.FinishHour,
			
			Name = request.Name,
			Description = request.Description,
			
			ClassroomId = classroom.Id,
			TeacherId = int.Parse(httpContext.User.FindFirst("Id")!.Value)
		};

		var hasIntersection = CheckIntersection(lessons, newLesson);
		if (hasIntersection)
		{
			return Results.BadRequest(new AddLessonResponse()
			{
				Error = "Classroom already reserved"
			});
		}

		try
		{
			await context.Lessons.AddAsync(newLesson);
			await context.SaveChangesAsync();
		}
		catch (DbUpdateException)
		{
			return Results.BadRequest(new AddLessonResponse()
			{
				Error = "Some error occurred"
			});
		}
		
		var path = httpContext.Request.Scheme + "://" + httpContext.Request.Host + "/Lesson/Get/" + hashids.Encode(newLesson.Id);
		return Results.Created(path, new AddLessonResponse());
	}
	
	private static bool CheckIntersection(List<Lesson> lessons, Lesson lesson)
	{
		var hourToReserve = Enumerable.Range(lesson.StartHour, lesson.FinishHour).ToArray();
		foreach (var l in lessons)
		{
			var reservedHours = Enumerable.Range(l.StartHour, l.FinishHour);
			if (reservedHours.Intersect(hourToReserve).Any())
			{
				return true;
			}
		}

		return false;
	}
}