using ContentService.Core.Contracts.Aggregates.Categories.Commands;
using ContentService.Core.Domain.Aggregates.Categories.ValueObjects;

using FluentValidation;

namespace ContentService.Core.ApplicationService.Aggregates.Categories.CommandHandlers;
public class CreateCategoryCommandValidation : AbstractValidator<CreateCategoryCommand>
{
	public CreateCategoryCommandValidation()
	{
		RuleFor(p => p.Title)
			.NotNull()
			.MinimumLength(CategoryTitle.Minimum)
			.MaximumLength(CategoryTitle.Maximum);
	}
}
