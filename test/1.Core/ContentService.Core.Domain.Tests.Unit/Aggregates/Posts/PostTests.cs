using ContentService.Core.Domain.Aggregates.Posts;

using EventBus.Messages.Aggregates.Posts.Events;

namespace ContentService.Core.Domain.Tests.Unit.Aggregates.Posts;
public class PostTests
{
	[Fact]
	public void CreatePost_ValidData_ShouldCreatePost()
	{
		// Arrange
		string title = "".PadLeft(50, '-');
		string description = "".PadLeft(160, '-');
		string text = "".PadLeft(300, '=');

		// Act
		var post = new Post()
			.Create(title, description, text);

		// Assert
		Assert.NotNull(post);
		Assert.Equal(title, post.Title.Value);
		Assert.Equal(description, post.Description.Value);
		Assert.Equal(text, post.Text.Value);
	}

	[Fact]
	public void CreatePost_InvalidData_ShouldNotCreatePost()
	{
		// Arrange
		string title = null;
		string description = null;
		string text = null;

		// Act
		var post = new Post()
			.Create(title, description, text);

		// Assert
		Assert.True(post.Result.IsFailed);
		Assert.Equal(3, post.Result.Errors.Count);
	}

	[Fact]
	public void CreatePost_ValidData_ShouldRaisePostCreatedEvent()
	{
		// Arrange
		string title = "".PadLeft(50, '-');
		string description = "".PadLeft(160, '-');
		string text = "".PadLeft(300, '=');

		// Act
		var post = new Post()
			.Create(title, description, text);

		// Assert
		var domainEvents = post.DomainEvents;
		Assert.Single(domainEvents);
		var postCreatedEvent = domainEvents.First() as PostCreatedEvent;
		Assert.NotNull(postCreatedEvent);
		Assert.Equal(post.Id, postCreatedEvent.Id);
		Assert.Equal(title, postCreatedEvent.Title);
		Assert.Equal(description, postCreatedEvent.Description);
		Assert.Equal(text, postCreatedEvent.Text);
	}

	[Fact]
	public void UpdatePost_ValidData_ShouldUpdatePost()
	{
		// Arrange
		string title = "".PadLeft(50, '-');
		string description = "".PadLeft(160, '-');
		string text = "".PadLeft(300, '=');
		var post = new Post()
			.Create(title, description, text);

		string newTitle = "".PadLeft(50, '-');
		string newDescription = "".PadLeft(160, '-');
		string newText = "".PadLeft(300, '=');

		// Act
		post.UpdatePost(newTitle, newDescription, newText);

		// Assert
		Assert.Equal(newTitle, post.Title.Value);
		Assert.Equal(newDescription, post.Description.Value);
		Assert.Equal(newText, post.Text.Value);
	}

	[Fact]
	public void RemovePost_ValidData_ShouldRemovePost()
	{
		// Arrange
		string title = "".PadLeft(50, '-');
		string description = "".PadLeft(160, '-');
		string text = "".PadLeft(300, '=');
		var post = new Post()
			.Create(title, description, text);

		// Act
		post.RemovePost(post.Id);

		// Assert
		var domainEvents = post.DomainEvents;
		Assert.Equal(2, domainEvents.Count);
		var postRemovedEvent = domainEvents.Last() as PostRemovedEvent;
		Assert.NotNull(postRemovedEvent);
		Assert.Equal(post.Id, postRemovedEvent.Id);
	}

	[Fact]
	public void ShouldBe_AddCategoryToPost_When_ValidData()
	{
		// Arrange
		string title = "".PadLeft(50, '-');
		string description = "".PadLeft(160, '-');
		string text = "".PadLeft(300, '=');
		var post = new Post()
			.Create(title, description, text);

		Guid categoryId = Guid.NewGuid();

		// Act
		post.AddCategory(categoryId);

		// Assert
		Assert.Contains(categoryId, post.CategoryIds.Select(c => c.Value));
		var domainEvents = post.DomainEvents;
		Assert.Equal(2, domainEvents.Count());
		var postCreatedEvent = domainEvents.First() as PostCreatedEvent;
		var postCategoryAddedEvent = domainEvents.Last() as PostCategoryAddedEvent;
		Assert.NotNull(postCreatedEvent);
		Assert.NotNull(postCategoryAddedEvent);
		Assert.Equal(post.Id, postCategoryAddedEvent.PostId);
		Assert.Equal(categoryId, postCategoryAddedEvent.CategoryId);
	}
	[Fact]
	public void ChangeCategory_WithValidData_ShouldRaiseCategoryPostChangedEvent()
	{
		// Arrange
		var oldCategoryId = Guid.NewGuid();
		var newCategoryId = Guid.NewGuid();

		var post = new Post()
			.Create("".PadLeft(50, '-'), "".PadLeft(160, '-'), "".PadLeft(300, '='));
		post.AddCategory(oldCategoryId);

		// Act
		post.ChangeCategory(oldCategoryId, newCategoryId);

		// Assert
		Assert.Equal(3, post.DomainEvents.Count);
		Assert.IsType<PostCreatedEvent>(post.DomainEvents[0]);
		var postCategoryAddedEvent = Assert.IsType<PostCategoryAddedEvent>(post.DomainEvents[1]);
		var categoryPostChangedEvent = Assert.IsType<CategoryPostChangedEvent>(post.DomainEvents[2]);
		Assert.Equal(categoryPostChangedEvent.OldCategoryId, oldCategoryId);
		Assert.Equal(categoryPostChangedEvent.NewCategoryId, newCategoryId);

	}

	[Fact]
	public void RemoveCategory_WithValidData_ShouldRaiseCategoryPostRemovedEvent()
	{
		// Arrange
		var categoryId = Guid.NewGuid();
		var post = new Post().Create("".PadLeft(50, '-'), "".PadLeft(160, '-'), "".PadLeft(300, '='));
		post.AddCategory(categoryId);

		// Act
		post.RemoveCategory(categoryId);

		// Assert
		Assert.Equal(3, post.DomainEvents.Count);
		Assert.IsType<PostCreatedEvent>(post.DomainEvents[0]);
		Assert.IsType<PostCategoryAddedEvent>(post.DomainEvents[1]);
		var postCategoryAddedEvent = Assert.IsType<CategoryPostRemovedEvent>(post.DomainEvents[2]);
		Assert.Equal(postCategoryAddedEvent.CategoryId, categoryId);
	}
	[Fact]
	public void AddCommentToThePost_ValidData_ShouldAddCommentToThePost()
	{
		// Arrange
		string title = "Test Title".PadLeft(50, '-');
		string description = "Test Description".PadLeft(150, '-');
		string text = "Test Text".PadLeft(255, '=');
		var post = new Post()
			.Create(title, description, text);

		string name = "Test Name";
		string email = "test@yahoo.com";
		string commentText = "Test Comment";

		// Act
		post.AddComment(name, email, commentText);


		// Assert
		Assert.True(post.Result.IsSuccess);
		Assert.Single(post.Comments);
		var comment = post.Comments.First();
		Assert.Equal(name, comment.Name.Value);
		Assert.Equal(email, comment.Email.Value);
		Assert.Equal(commentText, comment.CommentText.Value);
		var domainEvents = post.DomainEvents;
		Assert.Equal(domainEvents.Count, 2);
		var postCreatedEvent = domainEvents.First() as PostCreatedEvent;
		Assert.NotNull(postCreatedEvent);
		var commentAddedEvent = domainEvents.Last() as CommentAddedEvent;
		Assert.NotNull(commentAddedEvent);
		Assert.Equal(post.Id, commentAddedEvent.PostId);
		Assert.Equal(comment.Id, commentAddedEvent.CommentId);
		Assert.Equal(name, commentAddedEvent.DisplayName);
		Assert.Equal(email, commentAddedEvent.Email);
		Assert.Equal(commentText, commentAddedEvent.CommentText);
	}

	[Fact]
	public void EditComment_ValidData_ShouldEditComment()
	{
		// Arrange
		string title = "Test Title".PadLeft(50, '-');
		string description = "Test Description".PadLeft(150, '-');
		string text = "Test Text".PadLeft(255, '=');
		var post = new Post()
			.Create(title, description, text);

		string name = "Test Name";
		string email = "test@yahoo.com";
		string commentText = "Test Comment";
		post.AddComment(name, email, commentText);


		string newCommentText = "New Comment";

		// Act
		post.ChangeCommentText(name, email, commentText, newCommentText);

		// Assert
		Assert.True(post.Result.IsSuccess);
		Assert.Single(post.Comments);
		var comment = post.Comments.First();
		Assert.Equal(name, comment.Name.Value);
		Assert.Equal(email, comment.Email.Value);
		Assert.Equal(newCommentText, comment.CommentText.Value);
		var domainEvents = post.DomainEvents;
		Assert.Equal(domainEvents.Count, 3);
		var postCreatedEvent = domainEvents.First() as PostCreatedEvent;
		Assert.NotNull(domainEvents[1] as CommentAddedEvent);
		Assert.NotNull(postCreatedEvent);
		var commentEditedEvent = domainEvents.Last() as CommentEditedEvent;
		Assert.NotNull(commentEditedEvent);
		Assert.Equal(post.Id, commentEditedEvent.PostId);
		Assert.Equal(comment.Id, commentEditedEvent.CommentId);
		Assert.Equal(name, commentEditedEvent.DisplayName);
		Assert.Equal(email, commentEditedEvent.Email);
		Assert.Equal(newCommentText, commentEditedEvent.CommentText);
	}

	[Fact]
	public void RemoveComment_ValidData_ShouldRemoveComment()
	{
		// Arrange
		string title = "Test Title".PadLeft(50, '-');
		string description = "Test Description".PadLeft(150, '-');
		string text = "Test Text".PadLeft(255, '=');
		var post = new Post()
			.Create(title, description, text);

		string name = "Test Name";
		string email = "test@gmail.com";
		string commentText = "Test Comment";
		post.AddComment(name, email, commentText);

		// Act
		post.RemoveComment(name, email, commentText);

		// Assert
		Assert.True(post.Result.IsSuccess);
		Assert.Empty(post.Comments);
		var domainEvents = post.DomainEvents;
		Assert.Equal(domainEvents.Count, 3);
		var postCreatedEvent = domainEvents.First() as PostCreatedEvent;
		Assert.NotNull(postCreatedEvent);
		Assert.NotNull(domainEvents[1] as CommentAddedEvent);
		var commentRemovedEvent = domainEvents.Last() as CommentRemovedEvent;
		Assert.NotNull(commentRemovedEvent);
		Assert.Equal(post.Id, commentRemovedEvent.Id);
		Assert.Equal(name, commentRemovedEvent.Name);
		Assert.Equal(email, commentRemovedEvent.Email);
		Assert.Equal(commentText, commentRemovedEvent.Text);
	}
}
