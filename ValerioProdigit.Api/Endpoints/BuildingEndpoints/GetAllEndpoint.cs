using System.Net;
using Microsoft.EntityFrameworkCore;
using ValerioProdigit.Api.Data;
using ValerioProdigit.Api.Dtos.Building;
using ValerioProdigit.Api.Swagger;

namespace ValerioProdigit.Api.Endpoints.BuildingEndpoints;

public class GetAllEndpoint : IEndpointsMapper
{
    public void MapEndpoints(WebApplication app)
    {
        app.MapGet("/Building/GetAll", GetAll)
            .WithTags("Building")
            .WithDocumentation("Get all Building", "Get the list of all Building with their details")
            .WithResponseDocumentation<GetAllBuildingResponse>(HttpStatusCode.OK, "List of Building");
    }

    private static async Task<IResult> GetAll(AppDbContext dbContext)
    {
        var buildings = await dbContext.Buildings.AsNoTracking().ToListAsync();
        return Results.Ok(new GetAllBuildingResponse()
        {
            Buildings = buildings.Select(building => new GetAllBuildingResponse.Building()
            {
                Address = building.Address,
                Code = building.Code,
                Name = building.Name
            })
        });
    }
}