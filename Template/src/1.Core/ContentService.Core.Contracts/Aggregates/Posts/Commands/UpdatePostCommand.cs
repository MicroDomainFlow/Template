using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;

namespace ContentService.Core.Contracts.Aggregates.Posts.Commands;
public record UpdatePostCommand : ICommand<Guid>
{
	public required Guid PostId { get; init; }
	public required string Title { get; init; }
	public required string Description { get; init; }
	public required string Text { get; init; }
}
