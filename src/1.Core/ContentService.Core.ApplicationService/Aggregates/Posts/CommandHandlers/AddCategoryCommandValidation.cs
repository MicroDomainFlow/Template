using ContentService.Core.Contracts.Aggregates.Posts.Commands;

using FluentValidation;

namespace ContentService.Core.ApplicationService.Aggregates.Posts.CommandHandlers;
public class AddCategoryCommandValidation : AbstractValidator<AddCategoryCommand>
{
	public AddCategoryCommandValidation()
	{
		RuleFor(x => x.PostId)
			.NotEmpty()
			.NotNull();
		RuleFor(x => x.CategoryId)
			.NotEmpty()
			.NotNull();
	}
}
