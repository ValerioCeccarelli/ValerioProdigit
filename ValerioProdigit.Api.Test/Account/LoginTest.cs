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

public class LoginTest : IClassFixture<MyWebApplicationFactory>
{
	private readonly MyWebApplicationFactory _factory;

	public LoginTest(MyWebApplicationFactory factory)
	{
		_factory = factory;
	}

	[Fact]
	public async Task Login_WithValidCredentials_ReturnsOk()
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
		var loginRequest = new LoginRequest()
		{
			Email = "giovanni.giorgio@teacher.com",
			Password = "P@ssw0rd!"
		};
		var response = await client.PostAsJsonAsync("/Account/login", loginRequest);
		
		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
		loginResponse.Should().NotBeNull();
		loginResponse!.Success.Should().BeTrue();
		loginResponse!.Token.Should().NotBeNullOrEmpty();
	}
	
	[Fact]
	public async Task Login_WithNonValidCredentials_ReturnsBadRequest()
	{
		// Arrange
		var client = _factory.CreateClient();
		using var serviceScope = _factory.Services.CreateScope();
		var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
		var oldUser = new ApplicationUser()
		{
			Name = "Giovanni",
			Surname = "Giorgio",
			Email = "giovanni.giorgio@student.com",
			UserName = "giovanni.giorgio@student.com"
		};
		await userManager.CreateAsync(oldUser, "P@ssw0rd!");
		
		// Act
		var loginRequest = new LoginRequest()
		{
			Email = "giovanni.giorgiopppppp@student.com",
			Password = "P@ssw0rd!"
		};
		var response = await client.PostAsJsonAsync("/Account/login", loginRequest);
		
		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
		loginResponse.Should().NotBeNull();
		loginResponse!.Success.Should().BeFalse();
		loginResponse!.Error.Should().NotBeNullOrEmpty();
	}
}