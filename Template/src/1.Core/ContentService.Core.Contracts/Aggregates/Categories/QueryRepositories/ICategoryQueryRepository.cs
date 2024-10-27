using ContentService.Core.Contracts.Aggregates.Categories.Queries.GetAll;
using ContentService.Core.Contracts.Aggregates.Categories.Queries.GetCategoryById;
using ContentService.Core.Contracts.Aggregates.Categories.Queries.Models;


namespace ContentService.Core.Contracts.Aggregates.Categories.QueryRepositories;
public interface ICategoryQueryRepository
{
	public Task<List<CategoryQueryDto>> ExecuteAsync(GetAllCategoryQuery query, CancellationToken cancellationToken = default);
	public Task<List<CategoryQueryDto>> ExecuteAsync(GetAllSubCategoryQuery query, CancellationToken cancellationToken = default);
	public Task<CategoryQueryDto> ExecuteAsync(GetCategoryByIdQuery query, CancellationToken cancellationToken = default);
}
