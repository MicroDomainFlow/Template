using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;

namespace ContentService.Core.Contracts.Aggregates.Categories.Commands;
public readonly record struct AddParentCategoryCommand : ICommand<Guid>
{
	public Guid Id { get; init; }
	public Guid ParentCategoryId { get; init; }
}
