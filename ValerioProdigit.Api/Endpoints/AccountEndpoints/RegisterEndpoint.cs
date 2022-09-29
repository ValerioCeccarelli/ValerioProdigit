using System.Net;
using Microsoft.AspNetCore.Identity;
using ValerioProdigit.Api.Auth.Services;
using ValerioProdigit.Api.Configurations;
using ValerioProdigit.Api.Dtos.Account;
using ValerioProdigit.Api.Models;
using ValerioProdigit.Api.Swagger;
using ValerioProdigit.Api.Validators;
using ValerioProdigit.Api.Auth;

namespace ValerioProdigit.Api.Endpoints.AccountEndpoints;

public class RegisterEndpoint : IEndpointsMapper
{
    public void MapEndpoints(WebApplication app)
    {
        app.MapPost("/Account/Register", Register)
            .WithTags("Account")
            .WithDocumentation("Register new user", "Register new user and get the authentication token")
            .WithResponseDocumentation<RegisterResponse>(HttpStatusCode.OK, "Registration completed successfully")
            .WithResponseDocumentation<RegisterResponse>(HttpStatusCode.BadRequest, "Some of the provided data is invalid");
    }

    private static async Task<IResult> Register(
        RegisterRequest registerRequest,
        UserManager<ApplicationUser> userManager,
        IValidator<RegisterRequest> validator,
        EmailSettings emailSettings,
        JwtGenerator jwtGenerator)
    {
        var validationResult = validator.Validate(registerRequest);
        if (!validationResult.Succeeded)
        {
            return Results.BadRequest(new RegisterResponse()
            {
                Error = validationResult.Error
            });
        }

        var user = new ApplicationUser()
        {
            UserName = registerRequest.Email,
            Name = registerRequest.Name,
            Surname = registerRequest.Surname,
            Email = registerRequest.Email
        };

        var result = await userManager.CreateAsync(user, registerRequest.Password);
        if (!result.Succeeded)
        {
            return Results.BadRequest(new RegisterResponse
            {
                Error = result.Errors.First().Description
            });
        }

        var role = ChooseRole(user.Email, emailSettings);
        await userManager.AddToRoleAsync(user, role);

        var token = await jwtGenerator.Generate(user);

        return Results.Ok(new RegisterResponse()
        {
            Token = token
        });
    }
    
    private static string ChooseRole(string email, EmailSettings emailSettings)
    {
        var domain = email.Split('@')[1];
        if (emailSettings.AllowedAdminDomains.Contains(domain))
        {
            return Role.Admin;
        }
        else if (emailSettings.AllowedTeacherDomains.Contains(domain))
        {
            return Role.Teacher;
        }
        
        //else:
        return Role.Student;
        //this email is already valid, so if it is not an admin or Teacher then it is a Student
    }
}