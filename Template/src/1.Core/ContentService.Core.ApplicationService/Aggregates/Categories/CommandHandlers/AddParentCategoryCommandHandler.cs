using ContentService.Core.Contracts.Aggregates.Categories.CommandRepositories;
using ContentService.Core.Contracts.Aggregates.Categories.Commands;

using FluentResults;

using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;
using MDF.Resources.Common.FormattedMessages;

namespace ContentService.Core.ApplicationService.Aggregates.Categories.CommandHandlers;
public class AddParentCategoryCommandHandler : ICommandHandler<AddParentCategoryCommand, Guid>
{
	private readonly ICategoryCommandRepository _categoryCommandRepository;
	public AddParentCategoryCommandHandler(ICategoryCommandRepository categoryCommandRepository)
	{
		_categoryCommandRepository = categoryCommandRepository;
	}

	public async Task<Result<Guid>> Handle(AddParentCategoryCommand request, CancellationToken cancellationToken)
	{
		var category = await _categoryCommandRepository.GetGraphByAsync(request.Id, cancellationToken);
		if (category is null)
		{
			return Result.Fail(ErrorMessages.NotFound(request.ToString()));
		}
		category.AddParentCategory(request.ParentCategoryId);
		if (category.Result.IsFailed)
		{
			return category.Result;
		}
		_categoryCommandRepository.UpdateBy(category);
		await _categoryCommandRepository.CommitAsync(cancellationToken);
		return category.Id;
	}
}
