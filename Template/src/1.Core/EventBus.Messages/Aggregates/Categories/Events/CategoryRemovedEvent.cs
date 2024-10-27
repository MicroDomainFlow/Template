using MDF.Framework.SeedWork;

namespace EventBus.Messages.Aggregates.Categories.Events;

public record CategoryRemovedEvent(Guid Id, string? TitleValue, List<Guid> ParentCategoriesId) : IDomainEvent;