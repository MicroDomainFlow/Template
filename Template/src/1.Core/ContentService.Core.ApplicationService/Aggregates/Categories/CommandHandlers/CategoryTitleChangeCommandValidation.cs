using ContentService.Core.Contracts.Aggregates.Categories.Commands;
using ContentService.Core.Domain.Aggregates.Categories.ValueObjects;

using FluentValidation;

namespace ContentService.Core.ApplicationService.Aggregates.Categories.CommandHandlers;
public class CategoryTitleChangeCommandValidation : AbstractValidator<CategoryTitleChangeCommand>
{
	public CategoryTitleChangeCommandValidation()
	{
		RuleFor(p => p.Id).NotEmpty();
		RuleFor(p => p.Title)
			.NotNull()
			.MinimumLength(CategoryTitle.Minimum)
			.MaximumLength(CategoryTitle.Maximum);
	}
}
