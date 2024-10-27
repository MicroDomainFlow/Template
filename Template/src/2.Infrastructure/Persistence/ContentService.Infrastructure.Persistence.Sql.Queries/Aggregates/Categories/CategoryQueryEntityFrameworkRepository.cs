using AutoMapper;
using AutoMapper.QueryableExtensions;

using ContentService.Core.Contracts.Aggregates.Categories.Queries.GetAll;
using ContentService.Core.Contracts.Aggregates.Categories.Queries.GetCategoryById;
using ContentService.Core.Contracts.Aggregates.Categories.Queries.Models;
using ContentService.Core.Contracts.Aggregates.Categories.QueryRepositories;
using ContentService.Infrastructure.Persistence.Sql.Queries.Common;

using MDF.Framework.Infrastructure.Queries;

using Microsoft.EntityFrameworkCore;

namespace ContentService.Infrastructure.Persistence.Sql.Queries.Aggregates.Categories;
public class CategoryQueryEntityFrameworkRepository : BaseQueryRepository<ContentServiceQueryDbContext>, ICategoryQueryRepository
{
	private IMapper _mapper;
	public CategoryQueryEntityFrameworkRepository(ContentServiceQueryDbContext dbContext, IMapper mapper) : base(dbContext)
	{
		_mapper = mapper;
	}

	public Task<List<CategoryQueryDto>> ExecuteAsync(GetAllCategoryQuery query, CancellationToken cancellationToken = default)
	{
		return DbContext.Categories
					   .ProjectTo<CategoryQueryDto>(_mapper.ConfigurationProvider)
					   .ToListAsync(cancellationToken);
	}

	public async Task<List<CategoryQueryDto>> ExecuteAsync(GetAllSubCategoryQuery query, CancellationToken cancellationToken = default)
	{
		var allCategories = await DbContext.Categories.ToListAsync(cancellationToken);
		var filteredCategories = allCategories
			.Where(c => c.ParentCategoriesId.Any(pid => pid == query.CategoryId))
			.Select(c => _mapper.Map<CategoryQueryDto>(c))
			.ToList();
		return filteredCategories;

	}

	public Task<CategoryQueryDto> ExecuteAsync(GetCategoryByIdQuery query, CancellationToken cancellationToken = default)
	{
		return DbContext.Categories
			.Where(c => c.Id == query.Id)
			.ProjectTo<CategoryQueryDto>(_mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(cancellationToken);
	}
}
