using ContentService.Core.Contracts.Aggregates.Posts.Commands;
using ContentService.Core.Domain.Aggregates.Posts.ValueObjects;

using FluentValidation;

using MDF.Resources.Common;

namespace ContentService.Core.ApplicationService.Aggregates.Posts.CommandHandlers;
public class CreatePostCommandValidation : AbstractValidator<CreatePostCommand>
{
	public CreatePostCommandValidation()
	{
		RuleFor(p => p.Title)
			.NotNull()
			.MinimumLength(Title.Minimum)
			.MaximumLength(Title.Maximum)
			.WithName(DataDictionary.Title);
		RuleFor(p => p.Description)
			.NotNull()
			.MinimumLength(Description.Minimum)
			.MaximumLength(Description.Maximum)
			.WithName(DataDictionary.Description);
		RuleFor(p => p.Text)
			.NotNull()
			.MinimumLength(Text.Minimum)
			.WithName(DataDictionary.Text);
	}
}
