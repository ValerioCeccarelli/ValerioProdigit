using Swashbuckle.AspNetCore.Filters;
using ValerioProdigit.Api.Dtos.Account;

namespace ValerioProdigit.Api.Swagger.ExampleProviders.AccountExamples;

public sealed class ChangePasswordRequestExample : IExamplesProvider<ChangePasswordRequest>
{
    public ChangePasswordRequest GetExamples()
    {
        return new ChangePasswordRequest()
        {
            OldPassword = "P@ssw0rd!",
            NewPassword = "P@ssw0rd!2"
        };
    }
}