using MDF.Framework.SeedWork;

namespace EventBus.Messages.Aggregates.Posts.Events;

public class CommentAddedEvent : IDomainEvent
{
	public Guid PostId { get; }
	public Guid CommentId { get; }
	public string DisplayName { get; }
	public string Email { get; }
	public string CommentText { get; }

	public CommentAddedEvent(Guid postId, Guid commentId, string displayName, string email, string commentText)
	{
		PostId = postId;
		CommentId = commentId;
		DisplayName = displayName;
		Email = email;
		CommentText = commentText;
	}
}