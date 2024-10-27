using ContentService.Core.Domain.Aggregates.Posts.ValueObjects;

using MDF.Resources.Common;
using MDF.Resources.Common.FormattedMessages;

namespace ContentService.Core.Domain.Tests.Unit.Aggregates.Posts.ValueObjects;
public class CommentTextTests
{
	[Fact]
	public void ShouldBe_SuccessResult_When_ValidValue()
	{
		// Arrange
		string validValue = "This is a valid comment";

		// Act
		var result = CommentText.Create(validValue);

		// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(validValue, result.Value.Value);
		Assert.Empty(result.Errors);
		Assert.Empty(result.Successes);
		Assert.False(result.IsFailed);
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	public void ShouldBe_ErrorResult_When_NullOrEmptyValue(string? value)
	{
		// Act
		var result = CommentText.Create(value);
		var errorMessage = ValidationMessages.Required(DataDictionary.Comment);
		string errorMessage2 = ValidationMessages.Range(DataDictionary.Comment, CommentText.Maximum, CommentText.Minimum);
		// Assert
		Assert.True(result.IsFailed);
		Assert.False(result.IsSuccess);
		if (value is null)
		{
			Assert.Single(result.Errors);
			Assert.Equal(errorMessage, result.Errors[0].Message);
		}
		else
		{
			Assert.Equal(2, result.Errors.Count);
			Assert.Equal(errorMessage, result.Errors[0].Message);
			Assert.Equal(errorMessage2, result.Errors[1].Message);
		}
	}

	[Theory]
	[InlineData("short")]
	[InlineData("This comment is too long and exceeds the maximum allowed length This comment is too long and exceeds the maximum allowed lengthThis comment is too long and exceeds the maximum allowed lengthThis comment is too long and exceeds the maximum allowed lengthThis comment is too long and exceeds the maximum allowed lengthThis comment is too long and exceeds the maximum allowed lengthThis comment is too long and exceeds the maximum allowed lengthThis comment is too long and exceeds the maximum allowed lengthThis comment is too long and exceeds the maximum allowed lengthThis comment is too long and exceeds the maximum allowed lengthThis comment is too long and exceeds the maximum allowed length")]
	public void ShouldBe_ErrorResult_When_InvalidLengthValue(string value)
	{
		//Arrange
		string errorMessage = ValidationMessages.Range(DataDictionary.Comment, CommentText.Maximum, CommentText.Minimum);

		// Act
		var result = CommentText.Create(value);

		// Assert
		Assert.False(result.IsSuccess);
		Assert.True(result.IsFailed);
		Assert.Single(result.Errors);
		Assert.Equal(errorMessage, result.Errors[0].Message);
	}
}
