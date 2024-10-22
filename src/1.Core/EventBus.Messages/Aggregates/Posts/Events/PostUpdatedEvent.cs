using MDF.Framework.SeedWork;

namespace EventBus.Messages.Aggregates.Posts.Events;

public record PostUpdatedEvent(Guid Id, string Title, string Description, string Text) : IDomainEvent
{

}