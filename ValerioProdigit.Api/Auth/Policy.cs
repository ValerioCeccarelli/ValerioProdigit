using Microsoft.AspNetCore.Authorization;

namespace ValerioProdigit.Api.Auth;

public static class Policy
{
    public static void AddAdminPolicy(this AuthorizationOptions options)
    {
        options.AddPolicy(Admin, policyBuilder =>
        {
            policyBuilder.RequireRole(Role.Admin);
        });
    }

    public static void AddNonStudentPolicy(this AuthorizationOptions options)
    {
        options.AddPolicy(Policy.NonStudent, policyBuilder =>
        {
            policyBuilder.RequireRole(Role.Teacher, Role.Admin);
        });
    }
	
    public const string Admin = "AdminPolicy";

    public const string NonStudent = "NoStudentPolicy";
    
}