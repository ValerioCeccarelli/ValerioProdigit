using System.Net;
using HashidsNet;
using Microsoft.EntityFrameworkCore;
using ValerioProdigit.Api.Data;
using ValerioProdigit.Api.Dtos.Reservation;
using ValerioProdigit.Api.Swagger;

namespace ValerioProdigit.Api.Endpoints.ReservationEndpoints;

public sealed class GetEndpoint : IEndpointsMapper
{
	public void MapEndpoints(WebApplication app)
	{
		app.MapGet("/Reservation/Get/{reservationId}", GetById)
			.WithTags("Reservation")
			.WithDocumentation("Get reservation information","Get reservation information")
			.WithResponseDocumentation<GetReservationResponse>(HttpStatusCode.OK, "Reservation information")
			.WithResponseDocumentation<GetReservationResponse>(HttpStatusCode.BadRequest, "Invalid reservation id")
			.WithResponseDocumentation<GetReservationResponse>(HttpStatusCode.NotFound, "There is no reservation with the specified id");
	}

	private static async Task<IResult> GetById(
		string reservationId, 
		AppDbContext dbContext,
		IHashids hashids)
	{
		if (!hashids.TryDecodeSingle(reservationId, out var id))
		{
			return Results.BadRequest(new GetReservationResponse()
			{
				Error = "Invalid classroom id"
			});
		}

		var reservation = await dbContext.Reservations
			.AsNoTracking()
			.Include(r => r.Lesson)
			.Include(r => r.Student)
			.Where(r => r.Id == id)
			.FirstOrDefaultAsync();

		if (reservation is null)
		{
			return Results.NotFound(new GetReservationResponse()
			{
				Error = "Reservation not found"
			});
		}
        
		return Results.Ok(new GetReservationResponse()
		{
			Name = reservation.Student.Name,
			Surname = reservation.Student.Surname,
			Email = reservation.Student.Email,
			
			Row = reservation.Row,
			Seat = reservation.Seat,
			
			LessonName = reservation.Lesson.Name,
			LessonId = hashids.Encode(reservation.Lesson.Id)
		});
	}
}