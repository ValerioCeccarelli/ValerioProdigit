using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ValerioProdigit.Api.Dtos.Account;
using ValerioProdigit.Api.Models;
using Xunit;

namespace ValerioProdigit.Api.Test.Account;

public class RegisterTest : IClassFixture<MyWebApplicationFactory>
{
	private readonly MyWebApplicationFactory _factory;

	public RegisterTest(MyWebApplicationFactory factory)
	{
		_factory = factory;
	}

	[Fact]
	public async Task CreateNewUser_WithCorrectData_ReturnsOk()
	{
		// Arrange
		var client = _factory.CreateClient();
		using var serviceScope = _factory.Services.CreateScope();
		
		// Act
		var registerRequest = new RegisterRequest()
		{
			Name = "Giovanni",
			Surname = "Giorgio",
			Email = "giovanni.giorgio@admin.com",
			Password = "P@ssw0rd!"
		};
		var response = await client.PostAsJsonAsync("/Account/Register", registerRequest);
		
		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var registerResponse = await response.Content.ReadFromJsonAsync<RegisterResponse>();
		registerResponse.Should().NotBeNull();
		registerResponse!.Token.Should().NotBeNullOrWhiteSpace();
	}

	[Fact]
	public async Task CreateNewUser_WithExistingEmail_ReturnsBadRequest()
	{
		// Arrange
		var client = _factory.CreateClient();
		using var serviceScope = _factory.Services.CreateScope();
		var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
		var oldUser = new ApplicationUser()
		{
			Name = "Giovanni",
			Surname = "Giorgio",
			Email = "giovanni.giorgio@teacher.com",
			UserName = "giovanni.giorgio@teacher.com"
		};
		await userManager.CreateAsync(oldUser, "P@ssw0rd!");
		
		// Act
		var registerRequest = new RegisterRequest()
		{
			Name = "Giovanni",
			Surname = "Giorgio",
			Email = "giovanni.giorgio@teacher.com",
			Password = "P@ssw0rd!"
		};
		var response = await client.PostAsJsonAsync("/Account/Register", registerRequest);
		
		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		var registerResponse = await response.Content.ReadFromJsonAsync<RegisterResponse>();
		registerResponse.Should().NotBeNull();
		registerResponse!.Error.Should().NotBeNullOrWhiteSpace();
	}
}