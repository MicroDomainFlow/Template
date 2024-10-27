using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;

namespace ContentService.Core.Contracts.Aggregates.Categories.Commands;
public record CategoryTitleChangeCommand : ICommand<Guid>
{
	public required Guid Id { get; init; }
	public required string Title { get; init; }
}
