using ContentService.Core.Contracts.Aggregates.Posts.Commands.Comment;
using ContentService.Core.Domain.Aggregates.Posts.ValueObjects;

using FluentValidation;

using MDF.Resources.Common;

namespace ContentService.Core.ApplicationService.Aggregates.Posts.CommandHandlers.Comment;
public class AddCommentToPostCommandValidations : AbstractValidator<AddCommentToPostCommand>
{
	public AddCommentToPostCommandValidations()
	{
		RuleFor(c => c.CommentText)
			.NotEmpty()
			.MinimumLength(CommentText.Minimum)
			.MaximumLength(CommentText.Maximum)
			.WithName(DataDictionary.CommentText);
		RuleFor(c => c.DisplayName)
	.NotEmpty()
	.MinimumLength(DisplayName.Minimum)
	.MaximumLength(DisplayName.Maximum)
	.WithName(DataDictionary.Name);
		RuleFor(c => c.Email)
	.NotEmpty()
	.Matches(Email.EmailPattern)
	.WithName(DataDictionary.Email);

	}
}
