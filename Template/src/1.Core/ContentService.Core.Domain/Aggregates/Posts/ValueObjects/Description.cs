using FluentResults;

using FluentValidation;

using MDF.Framework.SeedWork;

namespace ContentService.Core.Domain.Aggregates.Posts.ValueObjects;
public class Description : BaseValueObject<string>
{
	public const byte Minimum = 150;
	public const byte Maximum = 160;

	//موقع استفاده از ef core متوجه خواهم شد که خطا میدهد یا خیر
	private Description() { }//for ef
	private Description(string value) : base(value)
	{
	}

	public static Result<Description?> Create(string? value)
	{
		Result<Description?> result = new();
		DescriptionValidator validator = new();
		var validationResult = validator.Validate(new Description(value));
		if (!validationResult.IsValid)
		{
			foreach (var failure in validationResult.Errors)
			{
				result.WithError(failure.ErrorMessage);
			}
		}
		if (result.IsFailed)
		{
			return result;
		}

		var returnValue = new Description(value);
		result.WithValue(returnValue);
		return result;
	}
}

public class DescriptionValidator : AbstractValidator<Description>
{
	public DescriptionValidator()
	{
		RuleFor(d => d.Value).NotEmpty().Length(Description.Minimum, Description.Maximum);
	}
}