using System.Net;
using HashidsNet;
using Microsoft.EntityFrameworkCore;
using ValerioProdigit.Api.Auth;
using ValerioProdigit.Api.Data;
using ValerioProdigit.Api.Dtos.Reservation;
using ValerioProdigit.Api.Emails;
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
		
		if (httpContext.User.IsInRole(Role.Admin))
		{
			return await DeleteIfAdmin(id, context);
		}
		else if (httpContext.User.IsInRole(Role.Teacher))
		{
			return await DeleteIfTeacher(id, context, httpContext);
		}
		else //Role.Student
		{
			return await DeleteIfStudent(id, context, httpContext);
		} 
	}

	private static async Task<IResult> DeleteIfAdmin(int id, AppDbContext context)
	{
		var reservation = await context.Reservations
			.Where(x => x.Id == id)
			.Include(x => x.Student)
			.FirstOrDefaultAsync();
		
		if (reservation is null)
		{
			return Results.NotFound(new DeleteReservationResponse()
			{
				Error = "Reservation not found"
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
	
	private static async Task<IResult> DeleteIfTeacher(int id, AppDbContext context, HttpContext httpContext)
	{
		var reservation = await context.Reservations
			.Where(x => x.Id == id)
			.Include(r => r.Lesson)
			.FirstOrDefaultAsync();
		
		if (reservation is null)
		{
			return Results.NotFound(new DeleteReservationResponse()
			{
				Error = "Reservation not found"
			});
		}
		
		var teacherId = int.Parse(httpContext.User.FindFirst("Id")!.Value);
		if (reservation.Lesson.TeacherId != teacherId)
		{
			return Results.BadRequest(new DeleteReservationResponse()
			{
				Error = "You can't delete a reservation that is not in your lesson"
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
	
	private static async Task<IResult> DeleteIfStudent(int id, AppDbContext context, HttpContext httpContext)
	{
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
		
		var studentId = int.Parse(httpContext.User.FindFirst("Id")!.Value);
		if (reservation.StudentId != studentId)
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