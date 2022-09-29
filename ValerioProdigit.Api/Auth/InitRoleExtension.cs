using Microsoft.AspNetCore.Identity;
using ValerioProdigit.Api.Models;

namespace ValerioProdigit.Api.Auth;

public static class InitRoleExtension
{
	public static async Task InitRolesAsync(this WebApplication app)
	{
		using var scope = app.Services.CreateScope();
		
		var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
		var logger = scope.ServiceProvider.GetRequiredService<ILogger<RoleManager<ApplicationRole>>>();
		
		var adminRoleExists = await roleManager.RoleExistsAsync(Role.Admin);
		if (!adminRoleExists)
		{
			var result = await roleManager.CreateAsync(new ApplicationRole() {Name = Role.Admin});
			if (!result.Succeeded)
			{
				logger.LogError("Error creating {role} role in database: {errors}", Role.Admin, result.Errors.First());
				throw new Exception($"Error creating {Role.Admin} role in database {result.Errors.First()}");
			}
		}
		
		var teacherRoleExists = await roleManager.RoleExistsAsync(Role.Teacher);
		if (!teacherRoleExists)
		{
			var result = await roleManager.CreateAsync(new ApplicationRole() {Name = Role.Teacher});
			if (!result.Succeeded)
			{
				logger.LogError("Error creating {role} role in database: {errors}", Role.Teacher, result.Errors.First());
				throw new Exception($"Error creating {Role.Teacher} role in database {result.Errors.First()}");
			}
		}
		
		var studentRoleExists = await roleManager.RoleExistsAsync(Role.Student);
		if (!studentRoleExists)
		{
			var result = await roleManager.CreateAsync(new ApplicationRole() {Name = Role.Student});
			if (!result.Succeeded)
			{
				logger.LogError("Error creating {role} role in database: {errors}", Role.Student, result.Errors.First());
				throw new Exception($"Error creating {Role.Student} role in database {result.Errors.First()}");
			}
		}
	}
}