using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ValerioProdigit.Api.Dtos.Account;

namespace ValerioProdigit.Api.Test;

public static class TestExtension
{
	public static async Task AuthenticateAsync(this HttpClient client, string email, string password)
	{
		var token = await client.GetJwtToken(email, password);
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
	}
	
	public static async Task<string> GetJwtToken(this HttpClient client, string email, string password)
	{
		var loginRequest = new LoginRequest()
		{
			Email = email,
			Password = password,
		};
		var response = await client.PostAsJsonAsync("/Account/login", loginRequest);
		//response.StatusCode.Should().Be(HttpStatusCode.OK);
		var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
		//loginResponse.Should().NotBeNull();
		return loginResponse!.Token;
	}
}