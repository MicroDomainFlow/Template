using ContentService.Core.Domain.Aggregates.Categories.ValueObjects;

namespace ContentService.Core.Domain.Tests.Unit.Aggregates.Categories.ValueObjects;
public class CategoryTitleTests
{
	[Fact]
	public void Should_ReturnSuccess_When_ValidValueProvided()
	{
		// Arrange
		string validValue = "ValidTitle";

		// Act
		var result = CategoryTitle.Create(validValue);

		// Assert
		Assert.True(result.IsSuccess);
		Assert.NotNull(result.Value);
		Assert.Equal(validValue, result.Value.Value);
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData(" ")]
	public void Should_ReturnFailure_When_InvalidValueProvided(string invalidValue)
	{
		// Act
		var result = CategoryTitle.Create(invalidValue);

		// Assert
		Assert.True(result.IsFailed);
		Assert.NotEmpty(result.Errors);
	}

	[Theory]
	[InlineData("Shor")] // shorter than 5
	[InlineData("ThisTitleIsTooLongToBeValid")]
	public void Should_ReturnFailure_When_ValueLengthIsInvalid(string invalidValue)
	{
		// Act
		var result = CategoryTitle.Create(invalidValue);

		// Assert
		Assert.True(result.IsFailed);
		Assert.NotEmpty(result.Errors);
	}

	[Fact]
	public void Should_ReturnSuccess_When_ValueLengthIsMinimum()
	{
		// Arrange
		string validValue = "Title";

		// Act
		var result = CategoryTitle.Create(validValue);

		// Assert
		Assert.True(result.IsSuccess);
		Assert.NotNull(result.Value);
		Assert.Equal(validValue, result.Value.Value);
	}

	[Fact]
	public void Should_ReturnSuccess_When_ValueLengthIsMaximum()
	{
		// Arrange
		string validValue = "ThisIsTheMaximumTitl";

		// Act
		var result = CategoryTitle.Create(validValue);

		// Assert
		Assert.True(result.IsSuccess);
		Assert.NotNull(result.Value);
		Assert.Equal(validValue, result.Value.Value);
	}
}
