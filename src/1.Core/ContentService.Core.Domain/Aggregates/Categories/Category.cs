using ContentService.Core.Domain.Aggregates.Categories.ValueObjects;

using EventBus.Messages.Aggregates.Categories.Events;
using EventBus.Messages.Aggregates.Posts.Events;

using MDF.Framework.Extensions.Guards;
using MDF.Framework.Extensions.Guards.GuardClauses;
using MDF.Framework.SeedWork;
using MDF.Framework.SeedWork.SharedKernel;
using MDF.Resources.Common;
using MDF.Resources.Common.FormattedMessages;

namespace ContentService.Core.Domain.Aggregates.Categories;

/// <summary>
/// Represents a category in the content service.
/// </summary>
public class Category : AggregateRoot<Category>
{
	public CategoryTitle CategoryTitle { get; private set; }

	private readonly List<Guid> _parentCategoriesId;
	public virtual IReadOnlyList<Guid> ParentCategoriesId => _parentCategoriesId;

	private List<Guid> _postIds;
	public virtual IReadOnlyList<Guid> PostIds => _postIds;

	public Category()
	{
		_postIds = new List<Guid>();
		_parentCategoriesId = new List<Guid>();
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Category"/> class.
	/// </summary>
	/// <param name="title">The title of the category.</param>
	private Category(string? title) : this()
	{
		// استفاده از گارد
		var titleResult = Guard.CheckIf(title, DataDictionary.Category)
			.NotNull()
			.NotEmpty()
			.LengthInclusiveBetween(5, 20);

		Result.WithErrors(titleResult._result.Errors);

		if (Result.IsSuccess)
		{
			CategoryTitle = CategoryTitle.Create(titleResult._result.Value).Value!;
		}
	}

	/// <summary>
	/// Creates a new category.
	/// </summary>
	/// <returns>The created category.</returns>
	public Category CreateCategory(string? title)
	{
		var checkValidation = new Category(title);
		Result.WithErrors(checkValidation.Result.Errors);

		if (Result.IsSuccess)
		{
			this.CategoryTitle = checkValidation.CategoryTitle;
			RaiseDomainEvent(new CategoryCreatedEvent(Id, this.CategoryTitle.Value, (List<Guid>)this.ParentCategoriesId));
		}
		return this;
	}

	/// <summary>
	/// Changes the title of the category.
	/// </summary>
	/// <param name="title">The new title of the category.</param>
	/// <returns>The updated category.</returns>
	public Category ChangeCategoryTitle(string? title)
	{
		// استفاده از گارد
		//var guardResult=Guard.CheckIf(title, DataDictionary.Title)
		//    .NotNull<string>()
		//    .NotEmpty<string>()
		//    .MinimumLength(Title.Minimum)
		//    .MaximumLength(Title.Maximum)
		//    .AsResult();

		// استفاده از Value Object
		var titleResult = CategoryTitle.Create(title);
		Result.WithErrors(titleResult.Errors);

		if (Result.IsSuccess)
		{
			this.CategoryTitle = titleResult.Value!;

			RaiseDomainEvent(new CategoryTitleChangedEvent(Id, CategoryTitle.Value));
			Result.WithSuccess(SuccessMessages.SuccessUpdate(DataDictionary.Post));
		}
		return this;
	}

	/// <summary>
	/// Removes a category.
	/// </summary>
	/// <param name="categoryId">The ID of the category to remove.</param>
	/// <returns>The updated category.</returns>
	public Category RemoveCategory(Guid? categoryId)
	{
		var guardResult = Guard.CheckIf(categoryId, DataDictionary.Category)
			.NotNull()
			.NotEmpty()
			.Length(36);

		Result.WithErrors(guardResult._result.Errors);


		if (Result.IsSuccess)
		{
			//اگر دسته بندی پدر باشد نمی توان آن را حذف کرد
			var foundedCategoryIsParent = this.ParentCategoriesId.Contains((Guid)categoryId);
			if (foundedCategoryIsParent)
			{
				Result.WithError(ErrorMessages.CanNotDelete(DataDictionary.Category));
				return this;
			}
			RaiseDomainEvent(new CategoryRemovedEvent(Id, this.CategoryTitle.Value, (List<Guid>)this.ParentCategoriesId));
		}

		return this;
	}

	/// <summary>
	/// Adds a parent category to the category.
	/// </summary>
	/// <param name="parentCategoryId">The ID of the parent category.</param>
	/// <returns>The updated category.</returns>
	public Category AddParentCategory(Guid? parentCategoryId)
	{
		var guardResult = Guard.CheckIf(parentCategoryId, DataDictionary.ParentCategory)
			.NotNull()
			.NotEmpty()
			.Length(36);

		Result.WithErrors(guardResult._result.Errors);

		if (Result.IsSuccess)
		{
			var currentCategoryIsParent = this.ParentCategoriesId.Contains((Guid)parentCategoryId);
			if (currentCategoryIsParent)
			{
				Result.WithError(ValidationMessages.Repetitive(DataDictionary.ParentCategory));
				return this;
			}

			_parentCategoriesId.Add((Guid)parentCategoryId);
			RaiseDomainEvent(new CategoryParentAddedEvent(Id, (List<Guid>)ParentCategoriesId));
		}
		return this;
	}

	/// <summary>
	/// Removes the parent category from the category.
	/// </summary>
	/// <returns>The updated category.</returns>
	public Category RemoveAllParentsCategory()
	{
		if (ParentCategoriesId.Count > 0)
		{
			_parentCategoriesId.Clear();
			RaiseDomainEvent(new CategoryParentRemovedEvent(Id, (List<Guid>)ParentCategoriesId));
		}

		return this;
	}


	public Category AddPostId(Guid? postId)
	{
		var guidGuard = GuidId.Create(postId);
		Result.WithErrors(guidGuard.Errors);

		if (Result.IsSuccess)
		{
			if (!_postIds.Contains((Guid)guidGuard.Value!))
			{
				_postIds.Add((Guid)guidGuard.Value!);
				RaiseDomainEvent(new CategoryPostAddedEvent(Id, (Guid)guidGuard.Value!));
			}

		}
		return this;
	}
}