using MDF.Framework.SeedWork;

namespace EventBus.Messages.Aggregates.Posts.Events;

public record CommentRemovedEvent(Guid Id, string Name, string Email, string Text) : IDomainEvent;