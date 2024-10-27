using FluentResults;

using MDF.Framework.SeedWork;
using MDF.Resources.Common;
using MDF.Resources.Common.FormattedMessages;

namespace ContentService.Core.Domain.Aggregates.Posts.ValueObjects;
//بدون استفاده از کتابخانه fluentValidation
public class Text : BaseValueObject<string>
{
	public const byte Minimum = byte.MaxValue;

	private Text() { }
	private Text(string value) : base(value)
	{
	}

	public static Result<Text?> Create(string? value)
	{
		Result<Text?> result = new();
		//اعتبار سنجی به صورت دستی بدون استفاده از گارد و fluent validation
		if (string.IsNullOrEmpty(value))
		{
			string errorMessage = ValidationMessages.Required(DataDictionary.Text);
			result.WithError(errorMessage);
		}

		if (value is not null && value.Length < Minimum)
		{
			string errorMessage = ValidationMessages.MinLength(DataDictionary.Text, Minimum);

			result.WithError(errorMessage);
		}

		if (result.IsFailed)
		{
			return result;
		}

		var returnValue = new Text(value!);
		result.WithValue(returnValue);
		return result;
	}
}
