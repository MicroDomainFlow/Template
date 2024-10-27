using ContentService.Core.Contracts.Aggregates.Categories.Queries.GetCategoryById;
using ContentService.Core.Contracts.Aggregates.Categories.Queries.Models;
using ContentService.Core.Contracts.Aggregates.Categories.QueryRepositories;

using FluentResults;

using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;
using MDF.Resources.Common;
using MDF.Resources.Common.FormattedMessages;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

using static MassTransit.ValidationResultExtensions;

using Result = FluentResults.Result;

namespace ContentService.Core.ApplicationService.Aggregates.Categories.QueriesHandlers.GetById;
public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryQueryDto>
{
	private readonly ICategoryQueryRepository _repository;
	private readonly ILogger<GetCategoryByIdQueryHandler> _logger;

	public GetCategoryByIdQueryHandler(ILogger<GetCategoryByIdQueryHandler> logger, ICategoryQueryRepository repository)
	{
		_logger = logger;
		_repository = repository;
	}

	public async Task<Result<CategoryQueryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
	{
		var category = await _repository.ExecuteAsync(request, cancellationToken);
		if (string.IsNullOrEmpty(category.CategoryTitle))
		{
			return Result.Fail(errorMessage: ErrorMessages.NotFound(DataDictionary.Post + "  id: " + request.Id));
		}
		return Result.Ok(category);
	}
}
