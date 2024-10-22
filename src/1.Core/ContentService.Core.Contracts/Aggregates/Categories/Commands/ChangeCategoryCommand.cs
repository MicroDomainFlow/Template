using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;

namespace ContentService.Core.Contracts.Aggregates.Categories.Commands;
public readonly record struct ChangeCategoryCommand : ICommand<Guid>
{
	public Guid PostId { get; init; }
	public Guid OldCategoryId { get; init; }
	public Guid NewCategoryId { get; init; }
}
