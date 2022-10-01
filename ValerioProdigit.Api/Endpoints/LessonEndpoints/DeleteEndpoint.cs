using System.Net;
using HashidsNet;
using Microsoft.EntityFrameworkCore;
using ValerioProdigit.Api.Auth;
using ValerioProdigit.Api.Data;
using ValerioProdigit.Api.Dtos.Lesson;
using ValerioProdigit.Api.Swagger;

namespace ValerioProdigit.Api.Endpoints.LessonEndpoints;

public sealed class DeleteEndpoint : IEndpointsMapper
{
	public void MapEndpoints(WebApplication app)
	{
		app.MapDelete("/Lesson/Delete/{lessonId}", Delete)
			.WithTags("Lesson")
			.RequireAuthorization(Policy.NonStudent)
			.WithDocumentation("Delete Lesson","Only admin users or the owner teacher can delete lesson")
			.WithResponseDocumentation(HttpStatusCode.Unauthorized, "You need a valid token to access this endpoint")
			.WithResponseDocumentation(HttpStatusCode.Forbidden, "You don't have the required permissions to access this endpoint (admin or teacher role required)")
			.WithResponseDocumentation<DeleteLessonResponse>(HttpStatusCode.OK, "Lesson deleted successfully")
			.WithResponseDocumentation<DeleteLessonResponse>(HttpStatusCode.BadRequest, "Some of the provided data is invalid")
			.WithResponseDocumentation<DeleteLessonResponse>(HttpStatusCode.NotFound, "Building, Classroom or Lesson not found");
	}

	private static async Task<IResult> Delete(
		string lessonId,
		AppDbContext context,
		HttpContext httpContext,
		IHashids hashids)
	{
		if(!hashids.TryDecodeSingle(lessonId, out var id))
		{
			return Results.BadRequest(new DeleteLessonResponse()
			{
				Error = "Invalid Lesson Id"
			});
		}

		var lesson = await context.Lessons
			.Where(l=>l.Id == id)
			.FirstOrDefaultAsync();

		if (lesson is null)
		{
			return Results.NotFound(new DeleteLessonResponse()
			{
				Error = "Lesson not found"
			});
		}

		var userId = httpContext.User.FindFirst("Id")!.Value;

		//you are the owner of the lesson OR you are an admin
		if (lesson.TeacherId.ToString() == userId || httpContext.User.IsInRole(Role.Admin))
		{
			try
			{
				context.Lessons.Remove(lesson);
				await context.SaveChangesAsync();
			}
			catch (DbUpdateException)
			{
				return Results.BadRequest(new DeleteLessonResponse()
				{
					Error = "Some error occurred"
				});
			}

			return Results.Ok(new DeleteLessonResponse());
		}

		return Results.BadRequest(new DeleteLessonResponse()
		{
			Error = "You are not allowed to delete this lesson"
		});
	}
}