using ContentService.Core.Domain.Aggregates.Posts.Entities;
using ContentService.Core.Domain.Aggregates.Posts.ValueObjects;

using EventBus.Messages.Aggregates.Posts.Events;

using MDF.Framework.SeedWork;
using MDF.Framework.SeedWork.SharedKernel;
using MDF.Resources.Common;
using MDF.Resources.Common.FormattedMessages;

namespace ContentService.Core.Domain.Aggregates.Posts;

public class Post : AggregateRoot<Post>
{
	public Title Title { get; private set; }
	public Description Description { get; private set; }
	public Text Text { get; private set; }
	private readonly List<GuidId> _categoryIds;
	public virtual IReadOnlyList<GuidId> CategoryIds => _categoryIds;
	#region بارگذاری تنبل در سطح دامنه
	private List<Comment> _comments;
	public virtual IReadOnlyList<Comment> Comments
	{
		get
		{
			if (_comments == null)
			{
				LoadComments();
			}
			return _comments.AsReadOnly();
		}
	}

	private void LoadComments()
	{
		// Load comments from the data source here.
		// This is just a placeholder. You will need to replace this with your actual data loading logic.
		_comments = new List<Comment>();
	}
	#endregion End بارگذاری تنبل در سطح دامنه

	public Post()
	{
		//_comments = new List<Comment>(); // برای بارگداری تنبل کامنت شده است
		_categoryIds = new List<GuidId>();
	}
	private Post(string? title, string? description, string? text) : this()
	{
		var titleResult = Title.Create(title);
		Result.WithErrors(titleResult.Errors);

		var descriptionResult = Description.Create(description);
		Result.WithErrors(descriptionResult.Errors);

		var contentResult = Text.Create(text);
		Result.WithErrors(contentResult.Errors);

		if (Result.IsSuccess)
		{
			Title = titleResult.Value;
			Description = descriptionResult.Value;
			Text = contentResult.Value;
		}
	}

	public Post Create(string? title, string? description, string? text)
	{
		var checkValidations = new Post(title, description, text);

		Result.WithErrors(checkValidations.Result.Errors);
		if (Result.IsFailed) return this;

		if (Result.IsSuccess)
		{
			this.Text = checkValidations.Text;
			this.Title = checkValidations.Title;
			this.Description = checkValidations.Description;
			RaiseDomainEvent(new PostCreatedEvent(Id, this.Title.Value, this.Description.Value, this.Text.Value));
		}
		return this;
	}
	public Post UpdatePost(string? title, string? description, string? text)
	{
		var checkValidations = new Post(title, description, text);
		Result.WithErrors(checkValidations.Result.Errors);
		if (Result.IsFailed) return this;

		if (Result.IsSuccess)
		{
			this.Title = checkValidations.Title;
			this.Description = checkValidations.Description;
			this.Text = checkValidations.Text;
			RaiseDomainEvent(new PostUpdatedEvent(Id, Title.Value!, Description.Value!, Text.Value!));
			Result.WithSuccess(SuccessMessages.SuccessUpdate(DataDictionary.Post));
		}

		return this;
	}
	public Post RemovePost(Guid? id)
	{
		var guidResult = GuidId.Create(id);

		if (guidResult.IsFailed)
		{
			Result.WithErrors(guidResult.Errors);
			return this;
		}
		//Note: if have IsDeleted property (soft delete) we can change to true here
		RaiseDomainEvent(new PostRemovedEvent(id));
		Result.WithSuccess(SuccessMessages.SuccessDelete(DataDictionary.Post));
		return this;
	}
	#region Category
	public Post AddCategory(Guid? categoryId)
	{
		var guidResult = GuidId.Create(categoryId);
		if (guidResult.IsFailed)
		{
			Result.WithErrors(guidResult.Errors);
			return this;
		}
		if (!_categoryIds.Contains(guidResult.Value))       //جلوگیری از تکراری بودن دسته بندی	
		{
			_categoryIds.Add(guidResult.Value);
			RaiseDomainEvent(new PostCategoryAddedEvent(Id, (Guid)categoryId!));
		}
		return this;
	}

	public Post ChangeCategory(Guid? oldCategoryId, Guid? newCategoryId)
	{
		var oldGuidResult = GuidId.Create(oldCategoryId);
		var newGuidResult = GuidId.Create(newCategoryId);
		if (oldGuidResult.IsFailed)
		{
			Result.WithErrors(oldGuidResult.Errors);
			return this;
		}
		if (newGuidResult.IsFailed)
		{
			Result.WithErrors(newGuidResult.Errors);
			return this;
		}

		if (_categoryIds.Contains(oldGuidResult.Value))
		{
			var indexOldCategory = _categoryIds.IndexOf(oldGuidResult.Value);
			if (!_categoryIds.Contains(newGuidResult.Value))
			{
				_categoryIds.RemoveAt(indexOldCategory);
				_categoryIds.Insert(indexOldCategory, newGuidResult.Value);
			}
			else
			{
				_categoryIds.RemoveAt(indexOldCategory);
			}

			RaiseDomainEvent(new CategoryPostChangedEvent(Id, (Guid)oldCategoryId!, (Guid)newCategoryId!));
		}
		else
		{
			Result.WithError(ErrorMessages.NotFound(DataDictionary.Category));
		}
		return this;
	}
	public Post RemoveCategory(Guid? categoryId)
	{
		var guidResult = GuidId.Create(categoryId);
		if (guidResult.IsFailed)
		{
			Result.WithErrors(guidResult.Errors);
			return this;
		}

		if (_categoryIds.Contains(guidResult.Value))
		{
			_categoryIds.Remove(guidResult.Value);
			RaiseDomainEvent(new CategoryPostRemovedEvent(Id, (Guid)categoryId!));
		}
		return this;
	}

	#endregion End Category

	#region Comments
	public Post AddComment(string? name, string? email, string? text)//به عنوان نمونه میباشد ممکن است منطق آن درست نباشد
	{

		var commentResult = Comment.Create(this, name, email, text);

		Result.WithErrors(commentResult.Errors);

		if (Result.IsFailed)
		{
			return this;
		}

		var hasAny = Comments
			.Any(c => c.Name == commentResult.Value.Name
					  && c.Email == commentResult.Value.Email
					  && c.CommentText == commentResult.Value.CommentText);

		if (hasAny)
		{
			var errorMessage = ValidationMessages.Repetitive(DataDictionary.Comment);
			Result.WithError(errorMessage);
			return this;
		}

		_comments.Add(commentResult.Value);
		RaiseDomainEvent(new CommentAddedEvent(this.Id, commentResult.Value.Id, commentResult.Value.Name.Value, commentResult.Value.Email.Value, commentResult.Value.CommentText.Value));
		return this;
	}
	public Post ChangeCommentText(string? name, string? email, string? text, string? newText)//به عنوان نمونه میباشد ممکن است منطق آن درست نباشد
	{
		var commentOldResult = Comment.Create(this, name, email, text);
		var commentNewResult = Comment.Create(this, name, email, newText);

		Result.WithErrors(commentOldResult.Errors);
		Result.WithErrors(commentNewResult.Errors);

		if (commentOldResult.Value.Email != commentNewResult.Value.Email)// change with guards
		{
			var errorMessage = ValidationMessages.Equality(commentOldResult.Value.Email.Value, commentNewResult.Value.Email.Value);
			Result.WithError(errorMessage);
		}
		if (commentOldResult.Value.Name != commentNewResult.Value.Name)// change with guards
		{
			var errorMessage = ValidationMessages.Equality(commentOldResult.Value.Name.Value, commentNewResult.Value.Name.Value);
			Result.WithError(errorMessage);
		}
		if (commentOldResult.Value.CommentText == commentNewResult.Value.CommentText)// change with guards
		{
			var errorMessage = ValidationMessages.Equality(commentOldResult.Value.CommentText.Value, commentNewResult.Value.CommentText.Value);
			Result.WithError(errorMessage);
		}

		if (Result.IsFailed)
		{
			return this;
		}
		var hasAny = Comments
			.Any(c => c.Name == commentNewResult.Value.Name
					  && c.Email == commentNewResult.Value.Email
					  && c.CommentText == commentNewResult.Value.CommentText);

		if (hasAny)
		{
			var errorMessage = ValidationMessages.Repetitive(DataDictionary.Comment);
			Result.WithError(errorMessage);
			return this;
		}

		//var commentIndex = _comments
		//	.FindIndex(c => c.Name == commentOldResult.Value.Name
		//			  && c.Email == commentOldResult.Value.Email
		//			  && c.CommentText == commentOldResult.Value.CommentText);
		var commentIndex = Comments
			.Select((c, i) => new { Comment = c, Index = i })
			.FirstOrDefault(x => x.Comment.Name == commentOldResult.Value.Name
								 && x.Comment.Email == commentOldResult.Value.Email
								 && x.Comment.CommentText == commentOldResult.Value.CommentText)?.Index;


		if (commentIndex >= 0)
		{
			_comments.RemoveAt((int)commentIndex);
			_comments.Insert((int)commentIndex, commentNewResult.Value);
			RaiseDomainEvent(new CommentEditedEvent(this.Id, commentNewResult.Value.Id, commentNewResult.Value.Name.Value, commentNewResult.Value.Email.Value, commentNewResult.Value.CommentText.Value));

		}


		return this;
	}
	public Post RemoveComment(string? name, string? email, string? text)
	{

		var commentResult = Comment.Create(this, name, email, text);
		Result.WithErrors(commentResult.Errors);
		if (Result.IsFailed)
		{
			return this;
		}

		var commentFounded = Comments
			.FirstOrDefault(c => c.Name?.Value?.ToLower() == commentResult.Value.Name?.Value?.ToLower()
								 && c.Email?.Value?.ToLower() == commentResult.Value?.Email?.Value?.ToLower()
								 && c.CommentText.Value?.ToLower() == commentResult?.Value?.CommentText.Value?.ToLower());

		if (commentFounded is null)
		{
			var errorMessage = ErrorMessages.NotFound(DataDictionary.Comment);
			Result.WithError(errorMessage);
			return this;
		}

		_comments.Remove(commentFounded);
		Result.WithSuccess(SuccessMessages.SuccessDelete(DataDictionary.Comment));
		RaiseDomainEvent(new CommentRemovedEvent(Id, name, email, text));
		return this;
	}
	#endregion
}