using MDF.Framework.SeedWork;

namespace EventBus.Messages.Aggregates.Posts.Events;

public record PostCategoryAddedEvent(Guid PostId, Guid CategoryId) : IDomainEvent;