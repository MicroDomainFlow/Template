using ContentService.Core.Contracts.Aggregates.Categories.Commands;

using FluentValidation;

namespace ContentService.Core.ApplicationService.Aggregates.Categories.CommandHandlers;
public class AddParentCategoryCommandValidation : AbstractValidator<AddParentCategoryCommand>
{
	public AddParentCategoryCommandValidation()
	{
		RuleFor(p => p.Id).NotEmpty();
		RuleFor(p => p.ParentCategoryId).NotEmpty();
	}
}
