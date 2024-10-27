using ContentService.Core.Domain.Aggregates.Posts.ValueObjects;

using MDF.Resources.Common;
using MDF.Resources.Common.FormattedMessages;

namespace ContentService.Core.Domain.Tests.Unit.Aggregates.Posts.ValueObjects;
public class EmailTests
{
	[Fact]
	public void ShouldBe_ReturnsSuccessResult_When_ValidEmail()
	{
		// Arrange
		string validEmail = "test@gmail.com";

		// Act
		var result = Email.Create(validEmail);

		// Assert
		Assert.True(result.IsSuccess);
		Assert.False(result.IsFailed);
		Assert.Empty(result.Errors);
		Assert.Empty(result.Successes);
	}

	[Fact]
	public void ShouldBe_ReturnsErrorResult_When_InvalidEmail()
	{
		// Arrange
		string invalidEmail = "invalidemail";
		var errorMessage = ValidationMessages.RegularExpression(DataDictionary.Email);

		// Act
		var result = Email.Create(invalidEmail);

		// Assert
		Assert.False(result.IsSuccess);
		Assert.True(result.IsFailed);
		Assert.Equal(errorMessage, result.Errors[0].Message);
		Assert.Single(result.Errors);
	}

	[Fact]
	public void ShouldBe_ReturnsErrorResult_When_NullEmail()
	{
		// Arrange
		string? nullEmail = null;
		var errorMessage = ValidationMessages.Required(DataDictionary.Email);
		var errorMessage2 = ValidationMessages.RegularExpression(DataDictionary.Email);

		// Act
		var result = Email.Create(nullEmail);

		// Assert
		Assert.True(result.IsFailed);
		Assert.False(result.IsSuccess);
		Assert.Equal(2, result.Errors.Count());
		Assert.Equal(errorMessage, result.Errors[0].Message);
		Assert.Equal(errorMessage2, result.Errors[1].Message);

	}

	[Fact]
	public void ShouldBe_ReturnsErrorResult_When_EmptyEmail()
	{
		// Arrange
		string emptyEmail = "";
		var errorMessage = ValidationMessages.Required(DataDictionary.Email);
		var errorMessage2 = ValidationMessages.RegularExpression(DataDictionary.Email);

		// Act
		var result = Email.Create(emptyEmail);

		// Assert
		Assert.True(result.IsFailed);
		Assert.False(result.IsSuccess);
		Assert.Equal(2, result.Errors.Count());
		Assert.Equal(errorMessage, result.Errors[0].Message);
		Assert.Equal(errorMessage2, result.Errors[1].Message);
	}
}
