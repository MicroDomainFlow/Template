using MDF.Framework.SeedWork;

namespace EventBus.Messages.Aggregates.Posts.Events;

public record CategoryPostRemovedEvent(Guid Id, Guid CategoryId) : IDomainEvent;