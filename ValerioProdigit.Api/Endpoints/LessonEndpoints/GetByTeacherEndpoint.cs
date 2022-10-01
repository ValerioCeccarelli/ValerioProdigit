using System.Net;
using HashidsNet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ValerioProdigit.Api.Data;
using ValerioProdigit.Api.Dtos.Lesson;
using ValerioProdigit.Api.Models;
using ValerioProdigit.Api.Swagger;
using ValerioProdigit.Api.Validators;

namespace ValerioProdigit.Api.Endpoints.LessonEndpoints;

public class GetByTeacherEndpoint : IEndpointsMapper
{
	public void MapEndpoints(WebApplication app)
	{
		app.MapPost("/Lesson/GetByTeacher", GetByTeacher)
			.WithTags("Lesson")
			.WithDocumentation("Get all Teacher's Lessons","Get the list of all Lessons of a Teacher in a given day")
			.WithResponseDocumentation<GetByTeacherResponse>(HttpStatusCode.OK, "List of lessons")
			.WithResponseDocumentation<GetByTeacherResponse>(HttpStatusCode.BadRequest, "Some of the provided data is invalid")
			.WithResponseDocumentation<GetByTeacherResponse>(HttpStatusCode.NotFound, "Teacher email not found");
	}

	private static async Task<IResult> GetByTeacher(
		[FromBody] GetByTeacherRequest request,
		AppDbContext context,
		IValidator<GetByTeacherRequest> validator,
		UserManager<ApplicationUser> userManager,
		IHashids hashids)
	{
		var validationResult = validator.Validate(request);
		if (!validationResult.Succeeded)
		{
			return Results.BadRequest(new GetByTeacherResponse()
			{
				Error = validationResult.Error
			});
		}

		//var user = await userManager.FindByEmailAsync(request.TeacherEmail);
		var user = await userManager.FindByNameAsync(request.TeacherEmail);

		if (user is null)
		{
			return Results.NotFound(new GetByTeacherResponse()
			{
				Error = "Teacher not found"
			});
		}

		var lessons = await context.Lessons
			.AsNoTracking()
			.Include(x => x.Classroom)
			.ThenInclude(x => x.Building)
			.Where(x => x.TeacherId == user.Id)
			.ToListAsync();

		return Results.Ok(new GetByTeacherResponse()
		{
			Lessons = lessons.Select(x => new LessonDto()
			{
				BuildingCode = x.Classroom.Code,
				ClassroomCode = x.Classroom.Building.Code,
				
				Date = x.Date.ToString("yyyy-MM-dd"),
				StartHour = x.StartHour,
				FinishHour = x.FinishHour,
				
				TeacherEmail = request.TeacherEmail,
				
				Name = x.Name,
				Description = x.Description,
				
				HashId = hashids.Encode(x.Id)
			})
		});
	}
}