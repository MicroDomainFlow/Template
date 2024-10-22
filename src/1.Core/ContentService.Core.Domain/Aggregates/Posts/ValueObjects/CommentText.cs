using FluentResults;

using MDF.Framework.SeedWork;
using MDF.Resources.Common;
using MDF.Resources.Common.FormattedMessages;

namespace ContentService.Core.Domain.Aggregates.Posts.ValueObjects;

public class CommentText : BaseValueObject<string>
{
	public const byte Minimum = 6;
	public const byte Maximum = byte.MaxValue;

	private CommentText() { }//for ef
	private CommentText(string? value) : base(value)
	{
	}
	public static Result<CommentText> Create(string? value)
	{
		Result<CommentText> result = new();
		if (string.IsNullOrEmpty(value))
		{
			string errorMessage = ValidationMessages.Required(DataDictionary.Comment);
			result.WithError(errorMessage);
		}
		if (value?.Length < Minimum || value?.Length > Maximum)
		{
			string errorMessage = ValidationMessages.Range(DataDictionary.Comment, Maximum, Minimum);
			result.WithError(errorMessage);
		}
		if (result.IsFailed)
		{
			return result;
		}

		var returnValue = new CommentText(value!);
		result.WithValue(returnValue);
		return result;
	}
}