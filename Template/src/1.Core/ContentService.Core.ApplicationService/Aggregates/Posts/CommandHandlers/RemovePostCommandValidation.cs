using ContentService.Core.Contracts.Aggregates.Posts.Commands;

using FluentValidation;

using MDF.Resources.Common;

namespace ContentService.Core.ApplicationService.Aggregates.Posts.CommandHandlers;
public class RemovePostCommandValidation : AbstractValidator<RemovePostCommand>
{
	public RemovePostCommandValidation()
	{
		RuleFor(p => p.PostId)
			.NotNull()
			.WithName(DataDictionary.Id);
	}
}
