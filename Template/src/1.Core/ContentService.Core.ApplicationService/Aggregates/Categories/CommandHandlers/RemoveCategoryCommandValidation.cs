using ContentService.Core.Contracts.Aggregates.Categories.Commands;

using FluentValidation;

namespace ContentService.Core.ApplicationService.Aggregates.Categories.CommandHandlers;
public class RemoveCategoryCommandValidation : AbstractValidator<RemoveCategoryCommand>
{
	public RemoveCategoryCommandValidation()
	{
		RuleFor(p => p.Id).NotEmpty();
	}
}
