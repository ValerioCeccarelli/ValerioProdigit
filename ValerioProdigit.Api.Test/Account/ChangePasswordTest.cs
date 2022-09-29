using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ValerioProdigit.Api.Dtos.Account;
using ValerioProdigit.Api.Models;
using Xunit;

namespace ValerioProdigit.Api.Test.Account;

//valid only if all LoginTest pass
public class ChangePasswordTest : IClassFixture<MyWebApplicationFactory>
{
	private readonly MyWebApplicationFactory _factory;

	public ChangePasswordTest(MyWebApplicationFactory factory)
	{
		_factory = factory;
	}
	
	[Fact]
	public async Task ChangePassword_WithValidCredential_ReturnsOk()
	{
		// Arrange
		var client = _factory.CreateClient();
		using var serviceScope = _factory.Services.CreateScope();
		var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
		await userManager.CreateAsync(new ApplicationUser()
		{
			Name = "Giovanni",
			Surname = "Giorgio",
			Email = "giovanni.giorgio@teacher.com",
			UserName = "giovanni.giorgio@teacher.com"
		}, "P@ssw0rd!");
		
		// Act
		await client.AuthenticateAsync("giovanni.giorgio@teacher.com", "P@ssw0rd!");
		var loginRequest = new ChangePasswordRequest()
		{
			OldPassword = "P@ssw0rd!",
			NewPassword = "P@ssw0rd!!1!XD",
		};
		var response = await client.PostAsJsonAsync("/Account/ChangePassword", loginRequest);
		
		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var loginResponse = await response.Content.ReadFromJsonAsync<ChangePasswordResponse>();
		loginResponse.Should().NotBeNull();
		loginResponse!.Success.Should().BeTrue();
	}

	[Fact]
	public async Task ChangePassword_WithNonValidOldPassword_ReturnsBadRequest()
	{
		// Arrange
		var client = _factory.CreateClient();
		using var serviceScope = _factory.Services.CreateScope();
		var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
		await userManager.CreateAsync(new ApplicationUser()
		{
			Name = "Giovanni",
			Surname = "Giorgio",
			Email = "giovanni.giorgio@student.com",
			UserName = "giovanni.giorgio@student.com"
		}, "P@ssw0rd!");
		
		// Act
		await client.AuthenticateAsync("giovanni.giorgio@student.com", "P@ssw0rd!");
		var loginRequest = new ChangePasswordRequest()
		{
			OldPassword = "P@ssw0rd!PassWrong",
			NewPassword = "P@ssw0rd!!1!XD",
		};
		var response = await client.PostAsJsonAsync("/Account/ChangePassword", loginRequest);
		
		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		var loginResponse = await response.Content.ReadFromJsonAsync<ChangePasswordResponse>();
		loginResponse.Should().NotBeNull();
		loginResponse!.Success.Should().BeFalse();
		loginResponse!.Error.Should().NotBeNullOrWhiteSpace();
	}
}