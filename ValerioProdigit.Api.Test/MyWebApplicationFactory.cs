using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using ValerioProdigit.Api.Data;
using ValerioProdigit.Api.Emails;
using ValerioProdigit.Api.Models;

namespace ValerioProdigit.Api.Test;

public class MyWebApplicationFactory : WebApplicationFactory<Program>
{
	private static int _counter;
	private static readonly ConcurrentDictionary<int, InMemoryDatabaseRoot> DatabaseRoots = new();
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureServices(services =>
		{
			DbConfiguration(services);
			EmailServiceConfiguration(services);
		});
	}

	public EmailSenderTestService EmailSenderTestService { get; } = new();
	private void EmailServiceConfiguration(IServiceCollection services)
	{
		var descriptor = services
			.SingleOrDefault(d => d.ServiceType == typeof(IEmailSender));
		services.Remove(descriptor!);
		services.AddSingleton<IEmailSender>(EmailSenderTestService);
	}

	private static void DbConfiguration(IServiceCollection services)
	{
		var descriptor = services
			.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

		services.Remove(descriptor!);

		var counter = _counter++;

		services.AddDbContext<AppDbContext>(options =>
		{
			//options.UseInMemoryDatabase($"InMemoryDbForTesting{_count++}");
			var ser = DatabaseRoots.GetOrAdd(counter, _ => new InMemoryDatabaseRoot());
			options.UseInMemoryDatabase("InMemoryDbForTesting", ser);
		});

		var sp = services.BuildServiceProvider();

		using var scope = sp.CreateScope();
		var scopedServices = scope.ServiceProvider;
		var db = scopedServices.GetRequiredService<AppDbContext>();

		db.Database.EnsureCreated();
	}
}

public class EmailSenderTestService : IEmailSender
{
	public bool SendRegisterConfirmationIsDelivered { get; set; }
	public Task<bool> SendRegisterConfirmation(ApplicationUser user, string link)
	{
		SendRegisterConfirmationIsDelivered = true;
		return Task.FromResult(true);
	}

	public bool SendReservationCreatedIsDelivered { get; set; }
	public Task<bool> SendReservationCreated(ApplicationUser user, Reservation reservation)
	{
		SendReservationCreatedIsDelivered = true;
		return Task.FromResult(true);
	}
}