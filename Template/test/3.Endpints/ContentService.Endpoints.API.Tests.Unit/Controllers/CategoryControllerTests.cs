using AutoMapper;

using ContentService.Core.Contracts.Aggregates.Categories.Commands;
using ContentService.Core.Contracts.Aggregates.Categories.Queries.GetAll;
using ContentService.Core.Contracts.Aggregates.Categories.Queries.GetCategoryById;
using ContentService.Core.Contracts.Aggregates.Categories.Queries.Models;
using ContentService.Endpoints.API.Controllers;
using ContentService.Endpoints.API.ViewModels.Categories;

using FluentResults;

using MDF.Framework.Extensions.Results;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Moq;

namespace ContentService.Endpoints.API.Tests.Unit.Controllers;
public class CategoryControllerTests
{
	private readonly Mock<IMediator> _mediatorMock;
	private readonly Mock<IMapper> _mapperMock;
	private readonly CategoryController _categoryController;

	public CategoryControllerTests()
	{
		_mediatorMock = new Mock<IMediator>();
		_mapperMock = new Mock<IMapper>();
		_categoryController = new CategoryController(_mediatorMock.Object, _mapperMock.Object);
	}

	[Fact]
	public async Task ShouldBe_ReturnsListOfCategories_When_GetAllCategoryAsync()
	{
		// Arrange
		var expectedCategories = new List<CategoryQueryDto>
							{
								new CategoryQueryDto { Id = Guid.NewGuid(), CategoryTitle = "Category 1" },
								new CategoryQueryDto { Id = Guid.NewGuid(), CategoryTitle = "Category 2" }
							};

		_mediatorMock.Setup(m => m.Send(It.IsAny<GetAllCategoryQuery>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedCategories);

		// Act
		var result = await _categoryController.GetAllCategoryAsync();

		// Assert
		var okResult = Assert.IsType<OkObjectResult>(result);
		var actualCategories = Assert.IsType<CustomResult<List<CategoryQueryDto>>>(okResult.Value);
		Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
		Assert.Equal(expectedCategories, actualCategories.Value);
	}

	[Fact]
	public async Task ShouldBe_ReturnsCategory_When_GetPostAsync()
	{
		// Arrange
		var categoryId = Guid.NewGuid();
		var expectedCategory = new CategoryQueryDto { Id = categoryId, CategoryTitle = "Category 1" };

		_mediatorMock.Setup(m => m.Send(It.IsAny<GetCategoryByIdQuery>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedCategory);

		// Act
		var result = await _categoryController.GetCategoryAsync(new GetCategoryByIdQuery { Id = categoryId });

		// Assert
		var okResult = Assert.IsType<OkObjectResult>(result);
		var actualCategory = Assert.IsType<CustomResult<CategoryQueryDto>>(okResult.Value);
		Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
		Assert.Equal(expectedCategory, actualCategory.Value);
	}

	[Fact]
	public async Task ShouldBe_ReturnsListOfSubCategories_When_GetCategoryIdAsync()
	{
		// Arrange
		var categoryId = Guid.NewGuid();
		var expectedSubCategories = new List<CategoryQueryDto>
							{
								new CategoryQueryDto { Id = Guid.NewGuid(), CategoryTitle = "Subcategory 1" },
								new CategoryQueryDto { Id = Guid.NewGuid(), CategoryTitle = "Subcategory 2" }
							};

		_mediatorMock.Setup(m => m.Send(It.IsAny<GetAllSubCategoryQuery>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedSubCategories);

		// Act
		var result = await _categoryController.GetSubCategoriesAsync(new GetAllSubCategoryQuery { CategoryId = categoryId });

		// Assert
		var okResult = Assert.IsType<OkObjectResult>(result);
		var actualSubCategories = Assert.IsType<CustomResult<List<CategoryQueryDto>>>(okResult.Value);
		Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
		Assert.Equal(expectedSubCategories, actualSubCategories.Value);
	}

	[Fact]
	public async Task ShouldBe_ReturnsCreatedCategoryId_When_CreateCategoryAsync()
	{
		// Arrange
		var createCategoryVm = new CreateCategoryCommandVm { CategoryTitle = "New Category" };
		var categoryId = Guid.NewGuid();

		_mapperMock.Setup(m => m.Map<CreateCategoryCommand>(createCategoryVm))
			.Returns(new CreateCategoryCommand { Title = createCategoryVm.CategoryTitle });

		_mediatorMock.Setup(m => m.Send(It.IsAny<CreateCategoryCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(categoryId);

		// Act
		var result = await _categoryController.CreateCategoryAsync(createCategoryVm);

		// Assert
		var okResult = Assert.IsType<OkObjectResult>(result);
		var actualCategoryId = Assert.IsType<CustomResult<Guid>>(okResult.Value);
		Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
		Assert.Equal(categoryId, actualCategoryId.Value);
	}

	[Fact]
	public async Task ShouldBe_ReturnsOkResult_When_CategoryTitleChangeAsync()
	{
		// Arrange
		var categoryId = Guid.NewGuid();
		var categoryTitleChangeCommand = new CategoryTitleChangeCommand { Id = categoryId, Title = "New Title" };

		_mediatorMock.Setup(m => m.Send(It.IsAny<CategoryTitleChangeCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Ok(categoryId));

		// Act
		var result = await _categoryController.CategoryTitleChangeAsync(categoryTitleChangeCommand);

		// Assert
		var okResult = Assert.IsType<OkObjectResult>(result);
		Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
	}

	[Fact]
	public async Task ShouldBe_ReturnsOkResult_When_DeletePostAsync()
	{
		// Arrange
		var removeCategoryCommand = new RemoveCategoryCommand { Id = Guid.NewGuid() };

		_mediatorMock.Setup(m => m.Send(It.IsAny<RemoveCategoryCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Ok());

		// Act
		var result = await _categoryController.DeleteCategoryAsync(removeCategoryCommand);

		// Assert
		var okResult = Assert.IsType<OkObjectResult>(result);
		Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
	}
}
