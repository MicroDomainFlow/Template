using MDF.Framework.SeedWork;

namespace EventBus.Messages.Aggregates.Categories.Events;

public record CategoryCreatedEvent(Guid Id, string Title, List<Guid> ParentCategoriesId) : IDomainEvent;