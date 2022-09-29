using System.Net;
using HashidsNet;
using Microsoft.EntityFrameworkCore;
using ValerioProdigit.Api.Auth;
using ValerioProdigit.Api.Data;
using ValerioProdigit.Api.Dtos.Reservation;
using ValerioProdigit.Api.Swagger;

namespace ValerioProdigit.Api.Endpoints.ReservationEndpoints;

public class DeleteEndpoint : IEndpointsMapper
{
	public void MapEndpoints(WebApplication app)
	{
		app.MapDelete("/Reservation/Delete/{reservationId}", DeleteAsync)
			.WithTags("Reservation")
			.RequireAuthorization()
			.WithDocumentation("Cancel a Reservation","Only an authenticated user can cancel his reservation")
			.WithResponseDocumentation(HttpStatusCode.Unauthorized, "You need a valid token to access this endpoint")
			.WithResponseDocumentation<AddReservationResponse>(HttpStatusCode.OK, "Reservation deleted successfully")
			.WithResponseDocumentation<AddReservationResponse>(HttpStatusCode.BadRequest, "Some of the provided data is invalid")
			.WithResponseDocumentation<AddReservationResponse>(HttpStatusCode.NotFound, "Building, Classroom, Teacher or Lesson not found");
	}

	private static async Task<IResult> DeleteAsync(
		string reservationId,
		AppDbContext context,
		HttpContext httpContext,
		IHashids hashids)
	{
		if (!hashids.TryDecodeSingle(reservationId, out var id))
		{
			return Results.BadRequest(new DeleteReservationResponse()
			{
				Error = "Invalid reservation id"
			});
		}
		
		var reservation = await context.Reservations
			.Where(x => x.Id == id)
			.FirstOrDefaultAsync();

		if (reservation is null)
		{
			return Results.NotFound(new DeleteReservationResponse()
			{
				Error = "Reservation not found"
			});
		}

		var userId = int.Parse(httpContext.User.FindFirst("Id")!.Value);
		if(httpContext.User.IsInRole(Role.Student) && reservation.StudentId != userId)
		{
			return Results.BadRequest(new DeleteReservationResponse()
			{
				Error = "You can't delete a reservation that is not yours"
			});
		}

		try
		{
			context.Reservations.Remove(reservation);
			await context.SaveChangesAsync();
		}
		catch (DbUpdateException)
		{
			return Results.BadRequest(new DeleteReservationResponse()
			{
				Error = "Some error occurred"
			});
		}
		
		return Results.Ok(new DeleteReservationResponse());
	}
}