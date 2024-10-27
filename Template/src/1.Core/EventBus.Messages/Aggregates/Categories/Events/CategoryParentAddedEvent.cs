using MDF.Framework.SeedWork;

namespace EventBus.Messages.Aggregates.Categories.Events;

public record CategoryParentAddedEvent(Guid Id, List<Guid> ParentCategoriesId) : IDomainEvent;