using ContentService.Core.Domain.Aggregates.Posts.ValueObjects;

using FluentResults;

using MDF.Resources.Common;
using MDF.Resources.Common.FormattedMessages;

namespace ContentService.Core.Domain.Tests.Unit.Aggregates.Posts.ValueObjects;
public class TextTests
{
	[Fact]
	public void Should_ReturnError_When_ValueNullOrEmpty()
	{
		Result<Text?> result = Text.Create(null);
		Assert.True(result.IsFailed);
		var validationMessageError = string.Format(ValidationMessages.Required(DataDictionary.Text));

		Assert.Equal(validationMessageError, result.Errors.First().Message);


		result = Text.Create("");
		Assert.True(result.IsFailed);
		Assert.Equal(validationMessageError, result.Errors.First().Message);
	}

	[Fact]
	public void Should_ReturnError_When_ValueIsLessThanMinimum()
	{
		Result<Text?> result = Text.Create("123456789");
		var validationMessageError = string.Format(ValidationMessages.MinLength(DataDictionary.Text, byte.MaxValue));

		Assert.True(result.IsFailed);
		Assert.Equal(validationMessageError, result.Errors.First().Message);
	}

	[Fact]
	public void Should_ReturnSuccess_When_ValueIsValid()
	{
		Result<Text?> result = Text.Create("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890");
		Assert.False(result.IsFailed);
		Assert.Equal("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890", result.Value.Value);
	}
}
