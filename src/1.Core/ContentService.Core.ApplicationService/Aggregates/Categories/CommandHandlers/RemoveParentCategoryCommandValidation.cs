using ContentService.Core.Contracts.Aggregates.Categories.Commands;

using FluentValidation;

namespace ContentService.Core.ApplicationService.Aggregates.Categories.CommandHandlers;
public class RemoveParentCategoryCommandValidation : AbstractValidator<RemoveParentCategoryCommand>
{
	public RemoveParentCategoryCommandValidation()
	{
		RuleFor(p => p.CategoryId)
			.NotEmpty()
			.NotNull();
	}
}
