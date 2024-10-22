using ContentService.Core.Domain.Aggregates.Categories;

using EventBus.Messages.Aggregates.Categories.Events;
using EventBus.Messages.Aggregates.Posts.Events;

using MDF.Resources.Common;
using MDF.Resources.Common.FormattedMessages;

namespace ContentService.Core.Domain.Tests.Unit.Aggregates.Categories;
public class CategoryTests
{
	[Fact]
	public void ShouldBe_CreateCategory_When_ValidTitleAndParentCategoryId()
	{
		// Arrange
		string title = "Category Title";
		Guid parentCategoryId = Guid.NewGuid();

		// Act
		var category = new Category();
		category.CreateCategory(title);
		category.AddParentCategory(parentCategoryId);

		// Assert
		Assert.True(category.Result.IsSuccess);
		Assert.Equal(title, category.CategoryTitle.Value);
		Assert.Equal(parentCategoryId, category.ParentCategoriesId.Single());
		Assert.Single(category.ParentCategoriesId);
		Assert.Contains(parentCategoryId, category.ParentCategoriesId);
		Assert.Contains(new CategoryCreatedEvent(category.Id, category.CategoryTitle.Value, (List<Guid>)category.ParentCategoriesId), category.DomainEvents);
	}

	[Fact]
	public void ShouldBe_CreateCategory_InvalidTitle_When_ReturnsFailureResult()
	{
		// Arrange
		string title = "in";
		Guid? parentCategoryId = Guid.NewGuid();

		// Act
		var category = new Category();
		category.AddParentCategory(parentCategoryId);
		category.CreateCategory(title);

		// Assert
		Assert.True(category.Result.IsFailed);
		Assert.Single(category.ParentCategoriesId);
		Assert.Empty(category.PostIds);
		Assert.Single(category.DomainEvents);
	}

	[Fact]
	public void ShouldBe_ChangeCategoryTitle_ValidTitle_When_ReturnsSuccessResult()
	{
		// Arrange
		string title = "New Category Title";
		var category = new Category()
			.CreateCategory("Old Category Title");
		category.ChangeCategoryTitle(title);
		// Act
		category.AddParentCategory(Guid.NewGuid());

		// Assert
		Assert.True(category.Result.IsSuccess);
		Assert.Equal(title, category.CategoryTitle.Value);
		Assert.Contains(new CategoryTitleChangedEvent(category.Id, category.CategoryTitle.Value), category.DomainEvents);
	}

	[Fact]
	public void ShouldBe_CanNotChangeCategoryTitle__When_InvalidTitle()
	{
		// Arrange
		string title = "Inva";
		var category = new Category()
			.CreateCategory("Old Category Title");

		// Act
		category.ChangeCategoryTitle(title);

		// Assert
		Assert.True(category.Result.IsFailed);
		Assert.NotEqual(title, category.CategoryTitle.Value);
		Assert.Single(category.DomainEvents);
	}

	[Fact]
	public void ShouldBe_AddParentCategory_ValidParentCategoryId_When_ReturnsSuccessResult()
	{
		// Arrange
		var parentCategoryId = Guid.NewGuid();
		var category = new Category()
			.CreateCategory("Category Title");

		// Act
		category.AddParentCategory(parentCategoryId);
		// Assert
		Assert.True(category.Result.IsSuccess);
		Assert.Equal(parentCategoryId, category.ParentCategoriesId.Single());
		Assert.Single(category.ParentCategoriesId);
		Assert.Contains(new CategoryParentAddedEvent(category.Id, (List<Guid>)category.ParentCategoriesId), category.DomainEvents);
	}

	[Fact]
	public void ShouldBe_ReturnsFailureResult_When_AddParentCategoryInvalidParentCategoryId()
	{
		// Arrange
		var parentCategoryId = Guid.Empty;
		var category = new Category()
			.CreateCategory("Category Title");

		// Act
		category.AddParentCategory(parentCategoryId);

		// Assert
		Assert.True(category.Result.IsFailed);
		Assert.Empty(category.ParentCategoriesId);
		Assert.Single(category.DomainEvents);
	}

	[Fact]
	public void ShouldBe_ReturnsSuccessResult_CategoryHasParentCategory_When_RemoveParentCategory()
	{
		// Arrange
		var parentCategory = new Category()
			.CreateCategory("Parent Category");
		var category = new Category()
			.CreateCategory("Category Title");

		category.AddParentCategory(parentCategory.Id);

		// Act
		category.RemoveAllParentsCategory();

		// Assert
		Assert.True(category.Result.IsSuccess);
		Assert.Empty(category.ParentCategoriesId);
		Assert.NotNull(category.ParentCategoriesId);
		Assert.Contains(new CategoryParentRemovedEvent(category.Id, (List<Guid>)category.ParentCategoriesId), category.DomainEvents);
	}

	[Fact]
	public void ShouldBe_RemoveParentCategoryNoActionAndReturnSuccessResult_When_CategoryDoesNotHaveParentCategory()
	{
		// Arrange
		var category = new Category()
			.CreateCategory("Category Title");

		// Act
		var result = category.RemoveAllParentsCategory();

		// Assert
		Assert.True(result.Result.IsSuccess);
		Assert.NotNull(category.ParentCategoriesId);
		Assert.Empty(category.ParentCategoriesId);
		Assert.Single(category.DomainEvents);
	}

	[Fact]
	public void ShouldBe_AddParentCategoryFailureResult_When_duplicateParentId()
	{
		// Arrange
		string errorMessage = ValidationMessages.Repetitive(DataDictionary.ParentCategory);

		var parentCategory = new Category()
			.CreateCategory("Parent Title");

		var category = new Category()
			.CreateCategory("Category1 Title");
		category.AddParentCategory(parentCategory.Id);

		var category2 = new Category()
			.CreateCategory("Category2 Title");
		// Act
		category2.AddParentCategory(parentCategory.Id);
		category2.AddParentCategory(parentCategory.Id);

		// Assert
		Assert.True(category2.Result.IsFailed);
		Assert.Equal(errorMessage, category2.Result.Errors[0].Message);
	}
	[Fact]
	public void ShouldBe_AddPostId_ValidPostId_When_ReturnsSuccessResult()
	{
		// Arrange
		var postId = Guid.NewGuid();
		var category = new Category()
			.CreateCategory("Category Title");

		// Act
		category.AddPostId(postId);

		// Assert
		Assert.True(category.Result.IsSuccess);
		Assert.Equal(postId, category.PostIds.Single());
		Assert.Single(category.PostIds);
		Assert.Contains(new CategoryPostAddedEvent(category.Id, postId), category.DomainEvents);
	}

	[Fact]
	public void ShouldBe_ReturnsFailureResult_When_AddPostIdInvalidPostId()
	{
		// Arrange
		var postId = Guid.Empty;
		var category = new Category()
			.CreateCategory("Category Title");

		// Act
		category.AddPostId(postId);

		// Assert
		Assert.True(category.Result.IsFailed);
		Assert.Empty(category.PostIds);
		Assert.Single(category.DomainEvents);
	}
}
