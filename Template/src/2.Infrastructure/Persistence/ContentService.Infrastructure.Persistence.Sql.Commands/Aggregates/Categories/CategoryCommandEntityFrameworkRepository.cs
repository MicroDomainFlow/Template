using ContentService.Core.Contracts.Aggregates.Categories.CommandRepositories;
using ContentService.Core.Domain.Aggregates.Categories;
using ContentService.Infrastructure.Persistence.Sql.Commands.Common;

using MDF.Framework.Infrastructure.Commands;

using Microsoft.EntityFrameworkCore;

namespace ContentService.Infrastructure.Persistence.Sql.Commands.Aggregates.Categories;
public class CategoryCommandEntityFrameworkRepository : BaseCommandEntityFrameworkRepository<Category, ContentServiceCommandDbContext>, ICategoryCommandRepository
{
	public CategoryCommandEntityFrameworkRepository(ContentServiceCommandDbContext dbContext) : base(dbContext)
	{
	}
	//  در ICategoryCommandRepository اضافه کرد
	//اینجا باید متد های مورد نیاز را نوشت

	public async Task<bool> IsParentCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
	{
		var allCategories = await DbContext.Categories.ToListAsync(cancellationToken);
		var filteredCategories = allCategories
			.Any(c => c.ParentCategoriesId.Any(pid => pid == categoryId));
		return filteredCategories;
	}
}
