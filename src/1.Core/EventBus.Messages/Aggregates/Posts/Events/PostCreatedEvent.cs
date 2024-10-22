using MDF.Framework.SeedWork;

namespace EventBus.Messages.Aggregates.Posts.Events;

public record PostCreatedEvent : IDomainEvent
{
	public Guid Id { get; init; }
	public string Title { get; init; }
	public string Description { get; init; }
	public string Text { get; init; }

	public PostCreatedEvent(Guid id, string title, string description, string text)
	{
		Id = id;
		Title = title;
		Description = description;
		Text = text;
	}
}