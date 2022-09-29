using System.Collections.Concurrent;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using ValerioProdigit.Api.Data;

namespace ValerioProdigit.Api.Test;

public class MyWebApplicationFactory : WebApplicationFactory<Program>
{
	private static int _counter;
	private static readonly ConcurrentDictionary<int, InMemoryDatabaseRoot> DatabaseRoots = new();
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureServices(services => 
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
		});
	}
}