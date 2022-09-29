using Microsoft.AspNetCore.Identity;
using ValerioProdigit.Api.Auth.Services;
using ValerioProdigit.Api.Dtos.Account;
using ValerioProdigit.Api.Models;
using ValerioProdigit.Api.Validators;

namespace ValerioProdigit.Api.Endpoints.AccountEndpoints;

public class RegisterConfirmationEndpoint : IEndpointsMapper
{
    public void MapEndpoints(WebApplication app)
    {
        app.MapPost("Account/RegisterConfirmation", RegisterConfirmation)
            .WithTags("Account");
    }

    private static async Task<IResult> RegisterConfirmation(
        RegisterConfirmationRequest request,
        UserManager<ApplicationUser> userManager, 
        JwtGenerator jwtGenerator,
        IValidator<RegisterConfirmationRequest> validator)
    {
        var validationResult = validator.Validate(request);
        if (!validationResult.Succeeded)
        {
            return Results.BadRequest(new RegisterConfirmationResponse()
            {
                Error = validationResult.Error
            });
        }

        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Results.BadRequest(new RegisterConfirmationResponse()
            {
                Error = "Invalid email"
            });
        }

        var isValidPassword = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isValidPassword)
        {
            return Results.BadRequest(new RegisterConfirmationResponse()
            {
                Error = "Invalid password"
            });
        }

        var confirmationResult = await userManager.ConfirmEmailAsync(user, request.Token);
        if(!confirmationResult.Succeeded)
        {
            return Results.BadRequest(new RegisterConfirmationResponse()
            {
                Error = confirmationResult.Errors.First().Code
            });
        }

        user.EmailConfirmed = true;
        var updateResult = await userManager.UpdateAsync(user);
        if(!updateResult.Succeeded)
        {
            return Results.BadRequest(new RegisterConfirmationResponse()
            {
                Error = updateResult.Errors.First().Code
            });
        }

        var jwt = await jwtGenerator.GenerateAsync(user);
        
        return Results.Ok(new RegisterConfirmationResponse()
        {
            Token = jwt
        });
    }
}