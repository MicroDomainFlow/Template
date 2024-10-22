using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ContentService.Infrastructure.Persistence.Sql.Commands.Common;
public class ContentServiceCommandDbContextFactory : IDesignTimeDbContextFactory<ContentServiceCommandDbContext>
{
	public ContentServiceCommandDbContext CreateDbContext(string[] args)
	{
		var builder = new DbContextOptionsBuilder<ContentServiceCommandDbContext>();

		var configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json")
			.Build();

		var connectionString = configuration.GetConnectionString("DefaultConnection");

		builder.UseSqlServer(connectionString);

		return new ContentServiceCommandDbContext(builder.Options);
	}
}
