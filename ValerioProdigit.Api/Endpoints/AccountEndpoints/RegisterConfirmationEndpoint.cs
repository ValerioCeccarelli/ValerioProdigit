using HashidsNet;
using Microsoft.AspNetCore.Identity;
using ValerioProdigit.Api.Models;

namespace ValerioProdigit.Api.Endpoints.AccountEndpoints;

public class RegisterConfirmationEndpoint : IEndpointsMapper
{
    public void MapEndpoints(WebApplication app)
    {
        app.MapPost("Account/RegisterConfirmation/{userId}/{token}", RegisterConfirmation)
            .WithTags("Account");
    }

    private static async Task<IResult> RegisterConfirmation(
        string userId,
        string token,
        UserManager<ApplicationUser> userManager,
        IHashids hashids)
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

        return Results.Ok("Account confirmed");
    }
}