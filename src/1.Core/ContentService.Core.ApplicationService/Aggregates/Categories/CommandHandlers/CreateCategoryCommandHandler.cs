using ContentService.Core.Contracts.Aggregates.Categories.CommandRepositories;
using ContentService.Core.Contracts.Aggregates.Categories.Commands;
using ContentService.Core.Domain.Aggregates.Categories;

using FluentResults;

using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;
using MDF.Resources.Common.FormattedMessages;

namespace ContentService.Core.ApplicationService.Aggregates.Categories.CommandHandlers;
public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, Guid>
{
	private readonly ICategoryCommandRepository _commandRepository;
	public CreateCategoryCommandHandler(ICategoryCommandRepository commandRepository)
	{
		_commandRepository = commandRepository;
	}

	public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
	{
		var category = new Category()
			.CreateCategory(request.Title);
		if (category.Result.IsSuccess)
		{
			await _commandRepository.InsertByAsync(category, cancellationToken);
			await _commandRepository.CommitAsync(cancellationToken);
			return category.Id;
		}
		return Result.Fail(ErrorMessages.UnexpectedError);
	}
}
