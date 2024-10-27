using ContentService.Core.Domain.Aggregates.Posts.ValueObjects;

namespace ContentService.Core.Domain.Tests.Unit.Aggregates.Posts.ValueObjects;
public class TitleTests
{
	[Fact]
	public void ShouldBe_Success_When_ValidInput()
	{
		// Arrange
		string? title = "01234567890123456789012345678901234567890123456789";

		// Act
		var result = Title.Create(title);

		// Assertion
		Assert.True(result.IsSuccess);
		Assert.False(result.IsFailed);
		Assert.Empty(result.Errors);
		Assert.Equal(title, result.Value.Value);
	}

	[Theory]
	[InlineData(null, 1, new string[] { "'Title' must not be empty." })]
	[InlineData("", 2, new string[] { "'Title' must not be empty.", "'Title' must be between 50 and 60 characters. You entered 0 characters." })]
	[InlineData("short title", 1, new string[] { "'Title' must be between 50 and 60 characters. You entered 11 characters." })]
	[InlineData("long title 0123456789012345678901234567890123456789012345678901234567890123456789", 1, new string[] { "'Title' must be between 50 and 60 characters. You entered 81 characters." })]
	public void ShouldBe_Error_When_InvalidInput(string? title, byte countOfError, string[] errorMessage)
	{
		// Arrange
		string? input = title;
		byte errorCount = countOfError;

		// Act
		var result = Title.Create(input);

		// Assertion
		Assert.False(result.IsSuccess);
		Assert.True(result.IsFailed);
		Assert.Equal(errorCount, result.Errors.Count());
		Assert.Equal(errorMessage[0], result.Errors[0].Message);
		if (countOfError == 2)
		{
			Assert.Equal(errorMessage[1], result.Errors[1].Message);
		}
	}
}
