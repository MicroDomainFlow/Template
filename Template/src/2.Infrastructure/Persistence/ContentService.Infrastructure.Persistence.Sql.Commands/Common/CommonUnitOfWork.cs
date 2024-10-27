using MDF.Framework.Infrastructure.Commands;

namespace ContentService.Infrastructure.Persistence.Sql.Commands.Common;
public class CommonUnitOfWork : BaseEntityFrameworkUnitOfWork<ContentServiceCommandDbContext>
{
	public CommonUnitOfWork(ContentServiceCommandDbContext dbContext) : base(dbContext)
	{
	}
}
