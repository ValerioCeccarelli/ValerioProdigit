using System.Net;
using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ValerioProdigit.Api.Data;
using ValerioProdigit.Api.Dtos.Reservation;
using ValerioProdigit.Api.Swagger;
using ValerioProdigit.Api.Validators;

namespace ValerioProdigit.Api.Endpoints.ReservationEndpoints;

public class GetMyEndpoint : IEndpointsMapper
{
	public void MapEndpoints(WebApplication app)
	{
		app.MapPost("/Reservation/GetMy", GetMyAsync)
			.WithTags("Reservation")
			.RequireAuthorization()
			.WithDocumentation("Get all my Reservation", "Only authenticated users can use this endpoint to get all their reservations in a day")
			.WithResponseDocumentation(HttpStatusCode.Unauthorized, "You need a valid token to access this endpoint")
			.WithResponseDocumentation<GetByLessonResponse>(HttpStatusCode.OK, "List of reservations")
			.WithResponseDocumentation<GetByLessonResponse>(HttpStatusCode.BadRequest, "Some of the provided data is invalid");
	}

	private static async Task<IResult> GetMyAsync(
		[FromBody] GetMyReservationRequest request,
		AppDbContext context,
		IValidator<GetMyReservationRequest> validator,
		HttpContext httpContext,
		IHashids hashids)
	{
		var validationResult = validator.Validate(request);
		if (!validationResult.Succeeded)
		{
			return Results.BadRequest(new GetMyReservationResponse()
			{
				Error = validationResult.Error
			});
		}
		
		var userId = httpContext.User.FindFirst("Id")!.Value;
		var reservations = await context.Reservations
			.Include(r => r.Lesson)
			.ThenInclude(l => l.Classroom)
			.ThenInclude(c => c.Building)
			.Include(r => r.Lesson.Teacher)
			.Where(r => r.StudentId == int.Parse(userId))
			.Where(r => r.Lesson.Date == request.Date)
			.ToListAsync();

		return Results.Ok(new GetMyReservationResponse()
		{
			Resevations = reservations.Select(x => new GetMyReservationResponse.ReservationDto()
			{
				LessonName = x.Lesson.Name,
				LessonDescription = x.Lesson.Description,
				TeacherEmail = x.Lesson.Teacher.Email,
				
				BuildingCode = x.Lesson.Classroom.Building.Code,
				ClassroomCode = x.Lesson.Classroom.Code,
				Address = x.Lesson.Classroom.Building.Address,
				
				Row = x.Row,
				Seat = x.Seat,
				
				Date = x.Lesson.Date,
				StartHour = x.Lesson.StartHour,
				FinishHour = x.Lesson.FinishHour,
				
				ReservationId = hashids.Encode(x.Id)
			})
		});
	}
}