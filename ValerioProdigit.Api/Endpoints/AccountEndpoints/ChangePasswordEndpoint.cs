using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ValerioProdigit.Api.Dtos.Account;
using ValerioProdigit.Api.Models;
using ValerioProdigit.Api.Swagger;
using ValerioProdigit.Api.Validators;

namespace ValerioProdigit.Api.Endpoints.AccountEndpoints;

public class ChangePasswordEndpoint : IEndpointsMapper
{
    public void MapEndpoints(WebApplication app)
    {
        app.MapPost("/Account/ChangePassword", ChangePassword)
            .RequireAuthorization()
            .WithTags("Account")
            .WithDocumentation("Change password", "")
            .WithResponseDocumentation(HttpStatusCode.Unauthorized, "You need a valid token to access this endpoint")
            .WithResponseDocumentation<ChangePasswordResponse>(HttpStatusCode.OK, "Password changed successfully")
            .WithResponseDocumentation<ChangePasswordResponse>(HttpStatusCode.BadRequest, "Some of the provided data is invalid");
    }

    [Authorize]
    private static async Task<IResult> ChangePassword(
        ChangePasswordRequest request,
        UserManager<ApplicationUser> userManager,
        IValidator<ChangePasswordRequest> validator,
        HttpContext httpContext)
    {
        var validationResult = validator.Validate(request);
        if (!validationResult.Succeeded)
        {
            return Results.BadRequest(new ChangePasswordResponse()
            {
                Error = validationResult.Error
            });
        }
        
        var id = httpContext.User.FindFirst("Id")!.Value;
        var user = await userManager.FindByIdAsync(id);

        var result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            return Results.BadRequest(new ChangePasswordResponse()
            {
                Error = result.Errors.First().Description
            });
        }
        
        return Results.Ok(new ChangePasswordResponse());
    }
}