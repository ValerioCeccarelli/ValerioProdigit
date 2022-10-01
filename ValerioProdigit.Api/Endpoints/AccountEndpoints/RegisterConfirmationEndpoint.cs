using HashidsNet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ValerioProdigit.Api.Logs;
using ValerioProdigit.Api.Models;

namespace ValerioProdigit.Api.Endpoints.AccountEndpoints;

public class RegisterConfirmationEndpoint : IEndpointsMapper
{
    public void MapEndpoints(WebApplication app)
    {
        app.MapGet("Account/RegisterConfirmation", RegisterConfirmation)
            .WithTags("Account");
    }

    private static async Task<IResult> RegisterConfirmation(
        [FromQuery] string userId,
        [FromQuery] string token,
        UserManager<ApplicationUser> userManager,
        IHashids hashids,
        ILogger<RegisterConfirmationEndpoint> logger)
    {
        if (!hashids.TryDecodeSingle(userId, out var id))
        {
            return Results.NotFound("Page not found");
        }
        
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is null)
        {
            return Results.NotFound("Page not found");
        }

        var confirmationResult = await userManager.ConfirmEmailAsync(user, token);
        if(!confirmationResult.Succeeded)
        {
            return Results.NotFound("Page not found");
        }

        user.EmailConfirmed = true;
        var updateResult = await userManager.UpdateAsync(user);
        if(!updateResult.Succeeded)
        {
            return Results.BadRequest("Something went wrong");
        }

        logger.LogUserCreated(user.Email);

        return Results.Ok("Account confirmed");
    }
}