using System.Net;
using HashidsNet;
using Microsoft.EntityFrameworkCore;
using ValerioProdigit.Api.Data;
using ValerioProdigit.Api.Dtos.Building;
using ValerioProdigit.Api.Swagger;

namespace ValerioProdigit.Api.Endpoints.BuildingEndpoints;

public class GetEndpoint : IEndpointsMapper
{
    public void MapEndpoints(WebApplication app)
    {
        app.MapGet("/Building/Get/{buildingId}", GetByCode)
            .WithTags("Building")
            .WithDocumentation("Get building information","Get building information with the list of classroom codes")
            .WithResponseDocumentation<GetBuildingResponse>(HttpStatusCode.OK, "Building information")
            .WithResponseDocumentation<GetBuildingResponse>(HttpStatusCode.BadRequest, "Invalid building id")
            .WithResponseDocumentation<GetBuildingResponse>(HttpStatusCode.NotFound, "There is no building with the specified code");
    }

    private static async Task<IResult> GetByCode(
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
            .AsNoTracking()
            .Include(x => x.Classrooms)
            .Where(b => b.Id == id)
            .FirstOrDefaultAsync();

        if (building is null)
        {
            return Results.NotFound(new GetBuildingResponse()
            {
                Error = "Building not found"
            });
        }
        
        return Results.Ok(new GetBuildingResponse()
        {
            Address = building.Address,
            Code = building.Code,
            Name = building.Name,
            ClassroomCodes = building.Classrooms.Select(x=>x.Code)
        });
    }
}