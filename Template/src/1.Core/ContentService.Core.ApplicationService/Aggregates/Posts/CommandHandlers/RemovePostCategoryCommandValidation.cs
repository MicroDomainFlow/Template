using ContentService.Core.Contracts.Aggregates.Posts.Commands.Comment;

using FluentValidation;

namespace ContentService.Core.ApplicationService.Aggregates.Posts.CommandHandlers;
public class RemovePostCategoryCommandValidation : AbstractValidator<RemovePostCategoryCommand>
{
	public RemovePostCategoryCommandValidation()
	{
		RuleFor(x => x.PostId)
			.NotEmpty()
			.NotNull();
		RuleFor(x => x.CategoryId)
			.NotEmpty()
			.NotNull();
	}
}
