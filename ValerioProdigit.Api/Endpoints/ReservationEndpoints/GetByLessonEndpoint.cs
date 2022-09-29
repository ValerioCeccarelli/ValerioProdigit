using System.Net;
using HashidsNet;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ValerioProdigit.Api.Auth;
using ValerioProdigit.Api.Data;
using ValerioProdigit.Api.Dtos.Reservation;
using ValerioProdigit.Api.Models;
using ValerioProdigit.Api.Swagger;

namespace ValerioProdigit.Api.Endpoints.ReservationEndpoints;

public class GetByLessonEndpoint : IEndpointsMapper
{
	public void MapEndpoints(WebApplication app)
	{
		app.MapGet("/Reservation/GetByLesson/{lessonId}", GetByLessonAsync)
			.WithTags("Reservation")
			.RequireAuthorization(Policy.NonStudent)
			.WithDocumentation("Get all Student in a Lesson","Only Admin and Teacher can use this endpoint to get all Student in a Lesson")
			.WithResponseDocumentation(HttpStatusCode.Unauthorized, "You need a valid token to access this endpoint")
			.WithResponseDocumentation(HttpStatusCode.Forbidden, "You don't have the required permissions to access this endpoint (admin or teacher role required)")
			.WithResponseDocumentation<GetByLessonResponse>(HttpStatusCode.OK, "List of reservations")
			.WithResponseDocumentation<GetByLessonResponse>(HttpStatusCode.BadRequest, "Some of the provided data is invalid")
			.WithResponseDocumentation<GetByLessonResponse>(HttpStatusCode.NotFound, "Lesson not found");
	}
	
	private static async Task<IResult> GetByLessonAsync(
		string lessonId,
		AppDbContext context,
		UserManager<ApplicationUser> userManager,
		IHashids hashids)
	{
		if(!hashids.TryDecodeSingle(lessonId, out var id))
		{
			return Results.BadRequest(new GetByLessonResponse()
			{
				Error = "Invalid Lesson HashId"
			});
		}

		var lesson = await context.Lessons
			.AsNoTracking()
			.Where(x => x.Id == id)
			.Include(x => x.Reservations)
			.ThenInclude(x => x.Student)
			.FirstOrDefaultAsync();
		
		if (lesson is null)
		{
			return Results.NotFound(new AddReservationResponse()
			{
				Error = "Lesson not found"
			});
		}
		
		return Results.Ok(new GetByLessonResponse()
		{
			Reservations = lesson.Reservations.Select(x => new GetByLessonResponse.ReservationDto()
			{
				Name = x.Student.Name,
				Surname = x.Student.Surname,
				Email = x.Student.Email,
				
				ReservationId = hashids.Encode(x.Id)
			})
		});
	}
}