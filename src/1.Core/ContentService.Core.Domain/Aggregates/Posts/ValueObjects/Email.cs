using FluentResults;

using MDF.Framework.SeedWork;
using MDF.Resources.Common;
using MDF.Resources.Common.FormattedMessages;

using System.Text.RegularExpressions;

namespace ContentService.Core.Domain.Aggregates.Posts.ValueObjects;

public class Email : BaseValueObject<string>
{
	private Email()
	{

	}
	private Email(string value) : base(value)
	{
	}
	public const string EmailPattern = @"^[a-zA-Z0-9._%+-]+@(gmail\.com|yahoo\.com|outlook\.com)$";

	public static Result<Email> Create(string? value)
	{
		Result<Email> result = new();
		if (string.IsNullOrEmpty(value))
		{
			var errorMessage = ValidationMessages.Required(DataDictionary.Email);
			result.WithError(errorMessage);
		}
		if (!Regex.IsMatch(value?.Trim() ?? string.Empty, EmailPattern))
		{
			var errorMessage = ValidationMessages.RegularExpression(DataDictionary.Email);
			result.WithError(errorMessage);
		}

		if (result.IsFailed)
			return result;

		var returnValue = new Email(value);
		result.WithValue(returnValue);
		return result;
	}
}