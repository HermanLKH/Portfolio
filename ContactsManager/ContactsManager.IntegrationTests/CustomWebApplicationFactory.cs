using DbContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CRUDTests
{
	// execute(copy) everything in Program.cs
	public class CustomWebApplicationFactory : WebApplicationFactory<Program>
	{
		// override configuration of Program.cs which handles http requests
		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			base.ConfigureWebHost(builder);

			// change to testing environment
			builder.UseEnvironment("Test");

			// change from real database to in-memory database
			builder.ConfigureServices(services =>
			{
				// descripter => represent service & its lifetime
				var descripter = services.SingleOrDefault(temp => temp.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

				// remove the real dbcontext service
				if(descripter != null)
				{
					services.Remove(descripter);
				}
				// add in-memory database service
				services.AddDbContext<ApplicationDbContext>(options =>
				{
					options.UseInMemoryDatabase("DatabaseForTesting");
				});
			});
		}
	}
}
