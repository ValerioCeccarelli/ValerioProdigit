using System.Net;
using HashidsNet;
using Microsoft.EntityFrameworkCore;
using ValerioProdigit.Api.Auth;
using ValerioProdigit.Api.Data;
using ValerioProdigit.Api.Dtos.Classroom;
using ValerioProdigit.Api.Logs;
using ValerioProdigit.Api.Swagger;

namespace ValerioProdigit.Api.Endpoints.ClassroomEndpoints;

public sealed class DeleteEndpoint : IEndpointsMapper
{
    public void MapEndpoints(WebApplication app)
    {
        app.MapDelete("/Classroom/Delete/{classroomId}", Delete)
            .WithTags("Classroom")
            .RequireAuthorization(Policy.Admin)
            .WithDocumentation("Delete Classroom","Only admin users can delete classrooms")
            .WithResponseDocumentation(HttpStatusCode.Unauthorized, "You need a valid token to access this endpoint")
            .WithResponseDocumentation(HttpStatusCode.Forbidden, "You don't have the required permissions to access this endpoint (admin role required)")
            .WithResponseDocumentation<DeleteClassroomResponse>(HttpStatusCode.OK, "Classroom deleted successfully")
            .WithResponseDocumentation<DeleteClassroomResponse>(HttpStatusCode.NotFound, "Classroom not found")
            .WithResponseDocumentation<DeleteClassroomResponse>(HttpStatusCode.BadRequest, "Some of the provided data is invalid");
    }

    private static async Task<IResult> Delete(
        string classroomId, 
        AppDbContext context,
        IHashids hashids,
        ILogger<DeleteEndpoint> logger)
    {
        if (!hashids.TryDecodeSingle(classroomId, out var id))
        {
            return Results.BadRequest(new DeleteClassroomResponse()
            {
                Error = "Invalid classroom id"
            });
        }

        var classroom = await context.Classrooms
            .Include(c => c.Building)
            .FirstOrDefaultAsync(x => x.Id == id);
        
        if (classroom is null)
        {
            return Results.NotFound(new DeleteClassroomResponse()
            {
                Error = "Classroom not found"
            });
        }

        try
        {
            context.Classrooms.Remove(classroom);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Results.BadRequest(new DeleteClassroomResponse()
            {
                Error = "Some error occurred"
            });
        }
        
        logger.LogClassroomDeleted(classroom);
        
        return Results.Ok(new DeleteClassroomResponse());
    }
}