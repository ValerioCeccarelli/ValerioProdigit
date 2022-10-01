using System.Net;
using System.Web;
using HashidsNet;
using Microsoft.AspNetCore.Identity;
using ValerioProdigit.Api.Dtos.Account;
using ValerioProdigit.Api.Emails;
using ValerioProdigit.Api.Models;
using ValerioProdigit.Api.Swagger;
using ValerioProdigit.Api.Validators;

namespace ValerioProdigit.Api.Endpoints.AccountEndpoints;

public class ResendConfirmationEmailEndpoint : IEndpointsMapper
{
	public void MapEndpoints(WebApplication app)
	{
		app.MapPost("Account/ResendConfirmationEmail", ResendConfirmationEmailAsync)
			.WithTags("Account")
			.WithDocumentation("Resend confirmation email", "Resend confirmation email to the user")
			.WithResponseDocumentation<ResendConfirmationResponse>(HttpStatusCode.OK, "Confirmation email sent")
			.WithResponseDocumentation<ResendConfirmationResponse>(HttpStatusCode.BadRequest, "Some errors occurred");
	}

	private static async Task<IResult> ResendConfirmationEmailAsync(
		ResendConfirmationRequest request,
		IValidator<ResendConfirmationRequest> validator,
		UserManager<ApplicationUser> userManager,
		IHashids hashids,
		HttpContext httpContext,
		IEmail emailSender)
	{
		var validationResult = validator.Validate(request);
		if (!validationResult.Succeeded)
		{
			return Results.BadRequest(new ResendConfirmationResponse()
			{
				Error = validationResult.Error
			});
		}
		
		var user = await userManager.FindByEmailAsync(request.Email);
		if (user is null)
		{
			return Results.BadRequest(new ResendConfirmationResponse()
			{
				Error = "Invalid Email"
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

		if (!user.EmailConfirmed)
		{
			return Results.BadRequest(new LoginResponse()
			{
				Error = "Email already confirmed"
			}); 
		}
		
		var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
		var encodedConfirmationToken = HttpUtility.UrlEncode(confirmationToken);
		var userId = hashids.Encode(user.Id);
		
		var link = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/Account/RegisterConfirmation?userId={userId}&token={encodedConfirmationToken}";

		var isEmailDelivered = await emailSender
			.SendRegisterConfirmation(user.Email, $"{user.Name} {user.Surname}", link);

		if (!isEmailDelivered)
		{
			return Results.BadRequest(new RegisterResponse()
			{
				Error = "Some errors occurs"
			});
		}

		return Results.Ok(new RegisterResponse());
	}
}