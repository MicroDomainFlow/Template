using MDF.Framework.SeedWork;

namespace EventBus.Messages.Aggregates.Posts.Events;

public record PostRemovedEvent(Guid? Id) : IDomainEvent;