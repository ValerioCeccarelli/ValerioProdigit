using System.Net;
using HashidsNet;
using Microsoft.EntityFrameworkCore;
using ValerioProdigit.Api.Auth;
using ValerioProdigit.Api.Data;
using ValerioProdigit.Api.Dtos.Building;
using ValerioProdigit.Api.Swagger;

namespace ValerioProdigit.Api.Endpoints.BuildingEndpoints;

public class DeleteEndpoint : IEndpointsMapper
{
    public void MapEndpoints(WebApplication app)
    {
        app.MapDelete("/Building/Delete/{buildingId}", Delete)
            .RequireAuthorization(Policy.Admin)
            .WithTags("Building")
            .WithDocumentation("Delete Building", "Only admin users can delete buildings")
            .WithResponseDocumentation(HttpStatusCode.Unauthorized, "You need a valid token to access this endpoint")
            .WithResponseDocumentation(HttpStatusCode.Forbidden, "You don't have the required permissions to access this endpoint (admin role required)")
            .WithResponseDocumentation<DeleteBuildingResponse>(HttpStatusCode.NotFound, "There is no building with the specified code")
            .WithResponseDocumentation<DeleteBuildingResponse>(HttpStatusCode.BadRequest, "Some of the provided data is invalid")
            .WithResponseDocumentation<DeleteBuildingResponse>(HttpStatusCode.OK, "Building deleted successfully");
    }

    private static async Task<IResult> Delete(
        string buildingId, 
        AppDbContext dbContext,
        IHashids hashids)
    {
        if (!hashids.TryDecodeSingle(buildingId, out var id))
        {
            return Results.BadRequest(new GetBuildingResponse()
            {
                Error = "Invalid building id"
            });
        }
        
        var building = await dbContext.Buildings
            .FirstOrDefaultAsync(x => x.Id == id);
        if (building is null)
        {
            return Results.NotFound(new DeleteBuildingResponse()
            {
                Error = "Building not found"
            });
        }

        try
        {
            dbContext.Buildings.Remove(building);
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Results.BadRequest(new DeleteBuildingResponse()
            {
                Error = "Some error occurred"
            });
        }

        return Results.Ok(new DeleteBuildingResponse());
    }
}