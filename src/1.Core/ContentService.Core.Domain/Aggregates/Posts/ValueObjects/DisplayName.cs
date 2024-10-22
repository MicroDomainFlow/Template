using FluentResults;

using MDF.Framework.SeedWork;
using MDF.Resources.Common;
using MDF.Resources.Common.FormattedMessages;

namespace ContentService.Core.Domain.Aggregates.Posts.ValueObjects;

public class DisplayName : BaseValueObject<string>
{
	private DisplayName()//for ef
	{

	}
	private DisplayName(string value) : base(value)
	{

	}
	public const byte Minimum = 5;
	public const byte Maximum = 15;

	public static Result<DisplayName> Create(string? value)
	{
		Result<DisplayName> result = new();

		if (string.IsNullOrEmpty(value))
		{
			var errorMessage = ValidationMessages.Required(DataDictionary.Name);
			result.WithError(errorMessage);
		}

		if (value?.Length < Minimum || value?.Length > Maximum)
		{
			string errorMessage = ValidationMessages.Range(DataDictionary.Name, Maximum, Minimum);
			result.WithError(errorMessage);
		}

		if (result.IsFailed) return result;

		var returnValue = new DisplayName(value);
		result.WithValue(returnValue);
		return result;
	}
}