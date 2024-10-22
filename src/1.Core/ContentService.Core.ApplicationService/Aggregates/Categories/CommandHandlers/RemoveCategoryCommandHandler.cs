using ContentService.Core.Contracts.Aggregates.Categories.CommandRepositories;
using ContentService.Core.Contracts.Aggregates.Categories.Commands;

using FluentResults;

using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;
using MDF.Resources.Common;
using MDF.Resources.Common.FormattedMessages;

namespace ContentService.Core.ApplicationService.Aggregates.Categories.CommandHandlers;
public class RemoveCategoryCommandHandler : ICommandHandler<RemoveCategoryCommand>
{
	private readonly ICategoryCommandRepository _categoryRepository;

	public RemoveCategoryCommandHandler(ICategoryCommandRepository categoryRepository)
	{

		_categoryRepository = categoryRepository;
	}

	public async Task<Result> Handle(RemoveCategoryCommand request, CancellationToken cancellationToken)
	{
		var category = await _categoryRepository.GetByAsync(request.Id, cancellationToken);
		if (category is null)
		{
			return Result.Fail(ErrorMessages.NotFound($"{DataDictionary.Category} - {request.Id}"));
		}
		var currentCategoryIsParent = await _categoryRepository.IsParentCategoryAsync(request.Id, cancellationToken);
		if (!currentCategoryIsParent)
		{
			category.RemoveCategory(request.Id);
			if (category.Result.IsSuccess)
			{
				_categoryRepository.DeleteBy(request.Id);
				await _categoryRepository.CommitAsync(cancellationToken);
				return Result.Ok(SuccessMessages.SuccessDelete($"{DataDictionary.Category} - {request.Id}")).ToResult();
			}
		}
		return Result.Fail(ErrorMessages.CanNotDelete($"{DataDictionary.Category}"));
	}
}
