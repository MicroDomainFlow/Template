#if (IncludeExample)
using ContentService.Core.Domain.Aggregates.Categories;
using ContentService.Core.Domain.Aggregates.Posts;
#endif
using MassTransit;

using MDF.Framework.Infrastructure.Commands;
using MDF.Framework.Infrastructure.Commands.ShadowProperties;
using MDF.Framework.Infrastructure.Conversions.Extensions;

using Microsoft.EntityFrameworkCore;

namespace ContentService.Infrastructure.Persistence.Sql.Commands.Common;
public class ContentServiceCommandDbContext : BaseCommandDbContext
{
#if (IncludeExample)
	public DbSet<Post> Posts { get; set; }
	public DbSet<Category> Categories { get; set; }
#endif
	public ContentServiceCommandDbContext(DbContextOptions<ContentServiceCommandDbContext> options) : base(options)
	{
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);
	}
	protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
	{
		base.ConfigureConventions(configurationBuilder);
		configurationBuilder.UseDateTimeAsUtcConversion();
		configurationBuilder.UseNullableDateTimeAsUtcConversion();
	}
	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.AddCommonShadowProperties();
		builder.ApplyConfigurationsFromAssembly
		(typeof(ContentServiceCommandDbContext).Assembly);

		//اضافه کردن OutBox pattern Masstransit
		builder.AddInboxStateEntity();
		builder.AddOutboxMessageEntity();
		builder.AddOutboxStateEntity();
	}
}