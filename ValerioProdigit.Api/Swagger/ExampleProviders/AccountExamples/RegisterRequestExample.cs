using Swashbuckle.AspNetCore.Filters;
using ValerioProdigit.Api.Dtos.Account;

namespace ValerioProdigit.Api.Swagger.ExampleProviders.AccountExamples;

public class RegisterRequestExample : IExamplesProvider<RegisterRequest>
{
    public RegisterRequest GetExamples()
    {
        return new RegisterRequest()
        {
            Email = "giovanni.giorgio@admin.com",
            Name = "Giovanni",
            Surname = "Giorgio",
            Password = "P@ssw0rd!",
        };
    }
}