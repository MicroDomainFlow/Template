using ContentService.Core.Contracts.Aggregates.Categories.Queries.GetAll;
using ContentService.Core.Contracts.Aggregates.Categories.Queries.Models;
using ContentService.Core.Contracts.Aggregates.Categories.QueryRepositories;

using FluentResults;

using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;

using Microsoft.Extensions.Logging;

namespace ContentService.Core.ApplicationService.Aggregates.Categories.QueriesHandlers.GetAll;
public class GetAllCategoryQueryHandler : IQueryHandler<GetAllCategoryQuery, List<CategoryQueryDto>>
{
	private readonly ICategoryQueryRepository _categoryQueryRepository;
	private readonly ILogger<GetAllCategoryQueryHandler> _logger;

	public GetAllCategoryQueryHandler(ILogger<GetAllCategoryQueryHandler> logger, ICategoryQueryRepository categoryQueryRepository)
	{
		_logger = logger;
		_categoryQueryRepository = categoryQueryRepository;
	}

	public async Task<Result<List<CategoryQueryDto>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
	{
		var categories = await _categoryQueryRepository.ExecuteAsync(request, cancellationToken);
		return Result.Ok(categories);
	}
}
