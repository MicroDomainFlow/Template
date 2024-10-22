using MDF.Framework.SeedWork.SharedKernel;

namespace ContentService.Core.Domain.Tests.Unit.SharedKernel;
public class GuidIdTests
{
	[Fact]
	public void Create_WithValidId_ShouldReturnSuccessResult()
	{
		// Arrange
		var id = Guid.NewGuid();

		// Act
		var result = GuidId.Create(id);

		// Assert
		Assert.True(result.IsSuccess);
		Assert.NotNull(result.Value);
		Assert.Equal(id, result.Value.Value);
	}

	[Fact]
	public void Create_WithNullId_ShouldReturnFailureResult()
	{
		// Arrange
		Guid? id = null;

		// Act
		var result = GuidId.Create(id);

		// Assert
		Assert.False(result.IsSuccess);
		Assert.NotEmpty(result.Errors);
	}

	[Fact]
	public void Create_WithEmptyId_ShouldReturnFailureResult()
	{
		// Arrange
		Guid? id = Guid.Empty;

		// Act
		var result = GuidId.Create(id);

		// Assert
		Assert.False(result.IsSuccess);
		Assert.NotEmpty(result.Errors);
	}

	[Fact]
	public void Create_WithInvalidLengthId_ShouldReturnFailureResult()
	{
		// Arrange
		var id = Guid.NewGuid().ToString().Substring(0, GuidId.FixLength - 1);

		// Act
		var result = GuidId.Create(id);

		// Assert
		Assert.False(result.IsSuccess);
		Assert.NotEmpty(result.Errors);
	}
}
