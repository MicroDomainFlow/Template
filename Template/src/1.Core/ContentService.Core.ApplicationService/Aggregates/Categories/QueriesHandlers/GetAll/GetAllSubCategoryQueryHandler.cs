using ContentService.Core.Contracts.Aggregates.Categories.Queries.GetAll;
using ContentService.Core.Contracts.Aggregates.Categories.Queries.Models;
using ContentService.Core.Contracts.Aggregates.Categories.QueryRepositories;


using FluentResults;

using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;

using Microsoft.Extensions.Logging;

namespace ContentService.Core.ApplicationService.Aggregates.Categories.QueriesHandlers.GetAll;
public class GetAllSubCategoryQueryHandler : IQueryHandler<GetAllSubCategoryQuery, List<CategoryQueryDto>>
{
	private readonly ICategoryQueryRepository _queryRepository;
	private readonly ILogger<GetAllSubCategoryQueryHandler> _logger;

	public GetAllSubCategoryQueryHandler(ILogger<GetAllSubCategoryQueryHandler> logger, ICategoryQueryRepository queryRepository)
	{
		_logger = logger;
		_queryRepository = queryRepository;
	}

	public async Task<Result<List<CategoryQueryDto>>> Handle(GetAllSubCategoryQuery request, CancellationToken cancellationToken)
	{
		var subCategories = await _queryRepository.ExecuteAsync(request, cancellationToken);
		return Result.Ok(subCategories);
	}
}
