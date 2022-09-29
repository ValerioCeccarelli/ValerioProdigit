using System.Net;
using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ValerioProdigit.Api.Auth;
using ValerioProdigit.Api.Data;
using ValerioProdigit.Api.Dtos.Classroom;
using ValerioProdigit.Api.Models;
using ValerioProdigit.Api.Swagger;
using ValerioProdigit.Api.Validators;

namespace ValerioProdigit.Api.Endpoints.ClassroomEndpoints;

public class AddEndpoint : IEndpointsMapper
{
    public void MapEndpoints(WebApplication app)
    {
        app.MapPost("/Classroom/Add", Add)
            .RequireAuthorization(Policy.Admin)
            .WithTags("Classroom")
            .WithDocumentation("Create new Classroom","Only admin users can create classrooms for a Building")
            .WithResponseDocumentation(HttpStatusCode.Unauthorized, "You need a valid token to access this endpoint")
            .WithResponseDocumentation(HttpStatusCode.Forbidden, "You don't have the required permissions to access this endpoint (admin role required)")
            .WithResponseDocumentation<AddClassroomResponse>(HttpStatusCode.Created, "Classroom created successfully")
            .WithResponseDocumentation<AddClassroomResponse>(HttpStatusCode.NotFound, "There is no building with the specified code")
            .WithResponseDocumentation<AddClassroomResponse>(HttpStatusCode.BadRequest, "Some of the provided data is invalid");
    }

    private static async Task<IResult> Add(
        [FromBody] AddClassroomRequest request, 
        AppDbContext context, 
        IValidator<AddClassroomRequest> validator,
        IHashids hashids,
        HttpContext httpContext)
    {
        var validationResult = validator.Validate(request);
        if (!validationResult.Succeeded)
        {
            return Results.BadRequest(new AddClassroomResponse()
            {
                Error = validationResult.Error
            });
        }

        var building = await context.Buildings
            .AsNoTracking()
            .Include(x => x.Classrooms)
            .FirstOrDefaultAsync(x => x.Code == request.BuildingCode);
        if (building is null)
        {
            return Results.NotFound(new AddClassroomResponse()
            {
                Error = "This building does not exist"
            });
        }

        if (building.Classrooms.Any(x => x.Code == request.ClassroomCode))
        {
            return Results.BadRequest(new AddClassroomResponse()
            {
                Error = "This code is already in use"
            });
        }

        var classroom = new Classroom()
        {
            BuildingId = building.Id,
            Capacity = request.Seats,
            Code = request.ClassroomCode,
            Rows = request.Rows.Select((seats, index) => new Row()
            {
                Index = index,
                Seats = seats
            }).ToList()
        };

        try
        {
            await context.Classrooms.AddAsync(classroom);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Results.BadRequest(new AddClassroomResponse()
            {
                Error = "Some error occurred"
            });
        }

        var path = httpContext.Request.Scheme + "://" + httpContext.Request.Host + "/Classroom/Get/" + hashids.Encode(classroom.Id);
        return Results.Created(path, new AddClassroomResponse());
    }
}