using MDF.Framework.SeedWork;

namespace EventBus.Messages.Aggregates.Posts.Events;

public record CategoryPostAddedEvent(Guid CategoryId, Guid PostId) : IDomainEvent;