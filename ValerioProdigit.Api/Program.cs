using ValerioProdigit.Api.Auth;
using ValerioProdigit.Api.Data;
using ValerioProdigit.Api.Emails;
using ValerioProdigit.Api.Endpoints;
using ValerioProdigit.Api.Hashids;
using ValerioProdigit.Api.Models;
using ValerioProdigit.Api.Swagger;
using ValerioProdigit.Api.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureAppDbContext();

builder.ConfigurePasswordOptions();
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.ConfigureHashId();
builder.ConfigureEmailService();

builder.ConfigureJwtAuthentication();
builder.ConfigureCustomAuthorization();

builder.ConfigureValidators();
builder.ConfigureSwagger();

var app = builder.Build();

await app.InitRolesAsync();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();
app.UseSwaggerEndpoint();

app.Run();

public partial class Program { }