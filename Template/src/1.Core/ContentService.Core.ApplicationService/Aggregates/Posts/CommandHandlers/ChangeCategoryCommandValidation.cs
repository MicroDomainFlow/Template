using ContentService.Core.Contracts.Aggregates.Categories.Commands;

using FluentValidation;

namespace ContentService.Core.ApplicationService.Aggregates.Posts.CommandHandlers;
public sealed class ChangeCategoryCommandValidation : AbstractValidator<ChangeCategoryCommand>
{
	public ChangeCategoryCommandValidation()
	{
		RuleFor(x => x.PostId)
			.NotEmpty()
			.NotNull();
		RuleFor(x => x.OldCategoryId)
			.NotEmpty()
			.NotNull();
		RuleFor(x => x.NewCategoryId)
			.NotEmpty()
			.NotNull();
	}
}
