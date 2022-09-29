using System.Net;
using HashidsNet;
using Microsoft.EntityFrameworkCore;
using ValerioProdigit.Api.Data;
using ValerioProdigit.Api.Dtos.Lesson;
using ValerioProdigit.Api.Swagger;

namespace ValerioProdigit.Api.Endpoints.LessonEndpoints;

public class GetEndpoint : IEndpointsMapper
{
	public void MapEndpoints(WebApplication app)
	{
		app.MapGet("Lesson/Get/{lessonId}", GetLesson)
			.WithTags("Lesson")
			.WithDocumentation("Get a Lesson", "Get information about a Lesson by its Id")
			.WithResponseDocumentation<GetLessonResponse>(HttpStatusCode.OK, "The Lesson was found and returned")
			.WithResponseDocumentation<GetLessonResponse>(HttpStatusCode.NotFound, "Lesson not found")
			.WithResponseDocumentation<GetLessonResponse>(HttpStatusCode.BadRequest, "Invalid lesson id");
	}

	private async Task<IResult> GetLesson(
		string lessonId, 
		AppDbContext context,
		IHashids hashids)
	{
		if (!hashids.TryDecodeSingle(lessonId, out var id))
		{
			return Results.BadRequest(new GetLessonResponse()
			{
				Error = "Invalid lesson id"
			});
		}

		var lesson = await context.Lessons
			.AsNoTracking()
			.Include(l => l.Classroom)
			.ThenInclude(c => c.Building)
			.Include(l => l.Teacher)
			.Where(l => l.Id == id)
			.FirstOrDefaultAsync();
		
		if(lesson is null)
		{
			return Results.NotFound(new GetLessonResponse()
			{
				Error = "Lesson not found"
			});
		}
		
		return Results.Ok(new GetLessonResponse()
		{
			Name = lesson.Name,
			Description = lesson.Description,
			BuildingCode = lesson.Classroom.Building.Code,
			ClassroomCode = lesson.Classroom.Code,
			StartHour = lesson.StartHour,
			FinishHour = lesson.FinishHour,
			HashId = lessonId,
			TeacherEmail = lesson.Teacher.Email
		});
	}
}