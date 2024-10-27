using MDF.Framework.SeedWork;

namespace EventBus.Messages.Aggregates.Posts.Events;

public record CategoryPostChangedEvent(Guid Id, Guid OldCategoryId, Guid NewCategoryId) : IDomainEvent;