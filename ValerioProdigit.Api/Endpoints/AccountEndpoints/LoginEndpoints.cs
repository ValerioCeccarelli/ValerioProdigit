using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ValerioProdigit.Api.Auth.Services;
using ValerioProdigit.Api.Dtos.Account;
using ValerioProdigit.Api.Models;
using ValerioProdigit.Api.Swagger;
using ValerioProdigit.Api.Validators;

namespace ValerioProdigit.Api.Endpoints.AccountEndpoints;

public class LoginEndpoints : IEndpointsMapper
{
    public void MapEndpoints(WebApplication app)
    {
        app.MapPost("/Account/Login", Login)
            .WithTags("Account")
            .WithDocumentation("Login","Get the authentication token if the user is valid")
            .WithResponseDocumentation<LoginResponse>(HttpStatusCode.OK, "Login completed successfully")
            .WithResponseDocumentation<LoginResponse>(HttpStatusCode.BadRequest, "Some of the provided data is invalid");
    }

    private static async Task<IResult> Login(
        [FromBody] LoginRequest request,
        IValidator<LoginRequest> validator,
        UserManager<ApplicationUser> userManager,
        JwtGenerator jwtGenerator)
    {
        var validationResult = validator.Validate(request);
        if (!validationResult.Succeeded)
        {
            return Results.BadRequest(new LoginResponse()
            {
                Error = validationResult.Error
            });
        }
        
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return Results.BadRequest(new LoginResponse()
            {
                Error = "Invalid email"
            }); 
        }

        var success = await userManager.CheckPasswordAsync(user, request.Password);

        if (!success)
        {
            return Results.BadRequest(new LoginResponse()
            {
                Error = "Invalid password"
            }); 
        }
        
        

        var token = await jwtGenerator.Generate(user);
		
        return Results.Ok(new LoginResponse()
        {
            Token = token
        });
    }
}