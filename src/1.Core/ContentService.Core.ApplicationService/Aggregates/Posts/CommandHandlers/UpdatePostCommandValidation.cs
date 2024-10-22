using ContentService.Core.Contracts.Aggregates.Posts.Commands;
using ContentService.Core.Domain.Aggregates.Posts.ValueObjects;

using FluentValidation;

using MDF.Resources.Common;

namespace ContentService.Core.ApplicationService.Aggregates.Posts.CommandHandlers;
public class UpdatePostCommandValidation : AbstractValidator<UpdatePostCommand>
{
	public UpdatePostCommandValidation()
	{
		RuleFor(p => p.PostId)
			.NotNull()
			.WithName(DataDictionary.Id);
		RuleFor(p => p.Title)
			.NotEmpty()
			.WithName(DataDictionary.Title)
			.MinimumLength(Title.Minimum)
			.MaximumLength(Title.Maximum);
		RuleFor(p => p.Description)
			.NotEmpty()
			.WithName(DataDictionary.Description)
			.MinimumLength(Description.Minimum)
			.MaximumLength(Description.Maximum); ;
		RuleFor(p => p.Text)
			.NotEmpty()
			.WithName(DataDictionary.Text)
			.MinimumLength(Text.Minimum);
	}
}
