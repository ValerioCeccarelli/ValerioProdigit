using System.Net;
using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ValerioProdigit.Api.Auth;
using ValerioProdigit.Api.Data;
using ValerioProdigit.Api.Dtos.Building;
using ValerioProdigit.Api.Logs;
using ValerioProdigit.Api.Models;
using ValerioProdigit.Api.Swagger;
using ValerioProdigit.Api.Validators;

namespace ValerioProdigit.Api.Endpoints.BuildingEndpoints;

public sealed class AddEndpoint : IEndpointsMapper
{
    public void MapEndpoints(WebApplication app)
    {
        app.MapPost("/Building/Add", Add)
            .RequireAuthorization(Policy.Admin)
            .WithTags("Building")
            .WithDocumentation("Create new Building", "Only admin users can create new buildings")
            .WithResponseDocumentation(HttpStatusCode.Unauthorized, "You need a valid token to access this endpoint")
            .WithResponseDocumentation(HttpStatusCode.Forbidden, "You don't have the required permissions to access this endpoint (admin role required)")
            .WithResponseDocumentation<AddBuildingResponse>(HttpStatusCode.Created, "Building created successfully")
            .WithResponseDocumentation<AddBuildingResponse>(HttpStatusCode.BadRequest, "Some of the provided data is invalid");
    }

    private static async Task<IResult> Add(
        [FromBody] AddBuildingRequest request,
        IValidator<AddBuildingRequest> validator,
        AppDbContext dbContext,
        IHashids hashids,
        HttpContext httpContext,
        ILogger<AddEndpoint> logger)
    {
        var validationResult = validator.Validate(request);
        if (!validationResult.Succeeded)
        {
            return Results.BadRequest(new AddBuildingResponse()
            {
                Error = validationResult.Error
            });
        }
        var alreadyExists = dbContext.Buildings.Any(b => b.Code == request.Code);
        if (alreadyExists)
        {
            return Results.BadRequest(new AddBuildingResponse()
            {
                Error = "This code is already in use"
            });
        }
        
        var building = new Building()
        {
            Address = request.Address,
            Code = request.Code,
            Name = request.Name
        };

        try
        {
            await dbContext.Buildings.AddAsync(building);
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Results.BadRequest(new AddBuildingResponse()
            {
                Error = "Some error occurred"
            });
        }
        
        logger.LogBuildingCreated(building);
        
        var path = httpContext.Request.Scheme + "://" + httpContext.Request.Host + "/Building/Get/" + hashids.Encode(building.Id);
        return Results.Created(path, new AddBuildingResponse());
    }
}