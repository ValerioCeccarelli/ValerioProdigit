using System.Net;
using HashidsNet;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ValerioProdigit.Api.Data;
using ValerioProdigit.Api.Dtos.Reservation;
using ValerioProdigit.Api.Emails;
using ValerioProdigit.Api.Models;
using ValerioProdigit.Api.Swagger;

namespace ValerioProdigit.Api.Endpoints.ReservationEndpoints;

public sealed class AddEndpoint : IEndpointsMapper
{
	public void MapEndpoints(WebApplication app)
	{
		app.MapPost("/Reservation/Add/{lessonId}", Add)
			.WithTags("Reservation")
			.RequireAuthorization()
			.WithDocumentation("Reserve a Lesson","Only authenticated users can reserve a seat for a lesson, only one seat per lesson can be reserved by the same user")
			.WithResponseDocumentation(HttpStatusCode.Unauthorized, "You need a valid token to access this endpoint")
			.WithResponseDocumentation<AddReservationResponse>(HttpStatusCode.Created, "Lesson reserved successfully")
			.WithResponseDocumentation<AddReservationResponse>(HttpStatusCode.BadRequest, "Some of the provided data is invalid or the lesson is already full")
			.WithResponseDocumentation<AddReservationResponse>(HttpStatusCode.NotFound, "Lesson not found");
	}

	private static async Task<IResult> Add(
		string lessonId,
		AppDbContext context,
		UserManager<ApplicationUser> userManager,
		HttpContext httpContext,
		IHashids hashids,
		IEmailSender emailSender)
	{
		if (!hashids.TryDecodeSingle(lessonId, out var id))
		{
			return Results.BadRequest(new AddReservationResponse()
			{
				Error = "Invalid HashId"
			});
		}

		var lesson = await context.Lessons
			.AsNoTracking()
			.Include(x => x.Classroom)
			.ThenInclude(c => c.Rows)
			.Where(x => x.Id == id)
			.FirstOrDefaultAsync();

		if (lesson is null)
		{
			return Results.NotFound(new AddReservationResponse()
			{
				Error = "Lesson not found"
			});
		}

		var classroom = lesson.Classroom;
		
		var reservations = await context.Reservations
			.AsNoTracking()
			.Where(x => x.LessonId == lesson.Id)
			.ToListAsync();

		if (reservations.Count >= classroom.Capacity)
		{
			return Results.BadRequest(new AddReservationResponse()
			{
				Error = "Classroom is full"
			});
		}
		
		var userId = int.Parse(httpContext.User.FindFirst("Id")!.Value);
		if(reservations.Any(x => x.StudentId == userId))
		{
			return Results.BadRequest(new AddReservationResponse()
			{
				Error = "You have already reserved this lesson"
			});
		}

		var rows = classroom.Rows.ToArray();
		var reservationsPerRow = reservations.GroupBy(x => x.Row);
		var emptyPlace = (0, 0);
		foreach (var pippo in reservationsPerRow)
		{
			var row = pippo.Key;
			var list = pippo.ToList();
			var dim = list.Count;
			if (dim < rows[row].Seats)
			{
				var seat = EmptySeatInRow(list);
				emptyPlace = (row, seat);
				break;
			}
		}
		
		var reservation = new Reservation()
		{
			Row = emptyPlace.Item1,
			Seat = emptyPlace.Item2,
			LessonId = lesson.Id,
			StudentId = userId
		};
		
		try
		{
			await context.Reservations.AddAsync(reservation);
			await context.SaveChangesAsync();
		}
		catch (DbUpdateException)
		{
			return Results.BadRequest(new AddReservationResponse()
			{
				Error = "Some error occurred"
			});
		}
		
		var user = await userManager.FindByIdAsync(userId.ToString());
		await emailSender.SendReservationCreated(user, reservation);
		
		var path = httpContext.Request.Scheme + "://" + httpContext.Request.Host + "/Reservation/Get/" + hashids.Encode(reservation.Id);
		return Results.Created(path, new AddReservationResponse());
	}

	private static int EmptySeatInRow(List<Reservation> reservations)
	{
		reservations.Sort();
		var pos = 0;
		foreach (var reservation in reservations)
		{
			if (pos != reservation.Seat)
			{
				return pos;
			}

			pos++;
		}
		
		return -1;
	}
}