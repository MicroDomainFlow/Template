using ContentService.Core.Domain.Aggregates.Posts.ValueObjects;

using MDF.Resources.Common;
using MDF.Resources.Common.FormattedMessages;

namespace ContentService.Core.Domain.Tests.Unit.Aggregates.Posts.ValueObjects;
public class DisplayNameTests
{
	[Fact]
	public void ShouldBe_SuccessResult_When_ValidValue()
	{
		// Arrange
		string validValue = "JohnDoe";

		// Act
		var result = DisplayName.Create(validValue);

		// Assert
		Assert.True(result.IsSuccess);
		Assert.False(result.IsFailed);
		Assert.Empty(result.Errors);
		Assert.Empty(result.Successes);
	}

	[Fact]
	public void ShouldBe_ErrorResult_When_NullValue()
	{
		// Arrange
		string? nullValue = null;
		var errorMessage = ValidationMessages.Required(DataDictionary.Name);

		// Act
		var result = DisplayName.Create(nullValue);

		// Assert
		Assert.True(result.IsFailed);
		Assert.Single(result.Errors);
		Assert.Equal(errorMessage, result.Errors[0].Message);
	}

	[Fact]
	public void ShouldBe_ErrorResult_When_EmptyValue()
	{
		// Arrange
		string emptyValue = "";
		var errorMessage = ValidationMessages.Required(DataDictionary.Name);
		string errorMessage2 = ValidationMessages.Range(DataDictionary.Name, DisplayName.Maximum, DisplayName.Minimum);

		// Act
		var result = DisplayName.Create(emptyValue);

		// Assert
		Assert.True(result.IsFailed);
		Assert.Equal(errorMessage, result.Errors[0].Message);
		Assert.Equal(errorMessage2, result.Errors[1].Message);
		Assert.Equal(2, result.Errors.Count);
	}

	[Fact]
	public void ShouldBe_ErrorResult_When_ShortValue()
	{
		// Arrange
		string shortValue = "Joe";
		string errorMessage = ValidationMessages.Range(DataDictionary.Name, DisplayName.Maximum, DisplayName.Minimum);

		// Act
		var result = DisplayName.Create(shortValue);

		// Assert
		Assert.True(result.IsFailed);
		Assert.Single(result.Errors);
		Assert.Equal(errorMessage, result.Errors[0].Message);
	}

	[Fact]
	public void ShouldBe_ErrorResult_When_LongValue()
	{
		// Arrange
		string longValue = "ThisIsAVeryLongName";
		string errorMessage = ValidationMessages.Range(DataDictionary.Name, DisplayName.Maximum, DisplayName.Minimum);

		// Act
		var result = DisplayName.Create(longValue);

		// Assert
		Assert.True(result.IsFailed);
		Assert.Single(result.Errors);
		Assert.Equal(errorMessage, result.Errors[0].Message);
	}
}
