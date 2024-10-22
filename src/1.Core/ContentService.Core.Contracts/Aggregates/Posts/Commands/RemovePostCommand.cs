using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;

namespace ContentService.Core.Contracts.Aggregates.Posts.Commands;
public struct RemovePostCommand : ICommand
{
	public Guid PostId { get; init; }
}
