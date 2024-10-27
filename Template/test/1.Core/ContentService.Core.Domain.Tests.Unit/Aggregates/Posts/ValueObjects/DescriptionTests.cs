using ContentService.Core.Domain.Aggregates.Posts.ValueObjects;
using FluentResults;

namespace ContentService.Core.Domain.Tests.Unit.Aggregates.Posts.ValueObjects;
public class DescriptionTests
{
	[Fact]
	public void ShouldBe_Success_When_ValidInput()
	{
		//Arrange
		string? description = "012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789";

		//Act
		var result = Description.Create(description);

		//Assertion
		Assert.True(result.IsSuccess);
		Assert.False(result.IsFailed);
		Assert.Empty(result.Errors);
		Assert.Equal(description, result.Value.Value);
	}

	//پیداکردن کدهای خطای کتابخانه fluentvalidation در آدرس زیر
	//https://github.com/FluentValidation/FluentValidation/tree/main/src/FluentValidation/Validators
	//خطا ها به صورت دستی اضافه شده است که این کار درستی نیست
	//https://docs.fluentvalidation.net/en/latest/built-in-validators.html
	[Theory]
	[InlineData(null, 1, new string[] { "'Value' must not be empty." })]
	[InlineData("", 2, new string[] { "'Value' must not be empty.", "'Value' must be between 150 and 160 characters. You entered 0 characters." })]
	[InlineData("short Description", 1, new string[] { "'Value' must be between 150 and 160 characters. You entered 17 characters." })]
	[InlineData("long Description 0123456789012345678901234560123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567897890123456789012345678901234567890123456789", 1, new string[] { "'Value' must be between 150 and 160 characters. You entered 237 characters." })]
	public void ShouldBe_Error_When_InvalidInput(string? title, byte countOfError, string[] errorMessage)
	{
		//Arrange
		string? input = title;
		byte errorCount = countOfError;

		//Act
		var result = Description.Create(input);

		//Assertion
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
