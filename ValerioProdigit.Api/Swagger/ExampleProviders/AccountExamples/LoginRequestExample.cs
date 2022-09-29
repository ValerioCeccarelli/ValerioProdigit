using Swashbuckle.AspNetCore.Filters;
using ValerioProdigit.Api.Dtos.Account;

namespace ValerioProdigit.Api.Swagger.ExampleProviders.AccountExamples;

public class LoginRequestExample : IExamplesProvider<LoginRequest>
{
    public LoginRequest GetExamples()
    {
        return new LoginRequest()
        {
            Email = "giovanni.giorgio@admin.com",
            Password = "P@ssw0rd!",
        };
    }
}