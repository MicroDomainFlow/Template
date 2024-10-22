using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;

namespace ContentService.Core.Contracts.Aggregates.Posts.Commands.Comment;
public readonly record struct RemovePostCategoryCommand : ICommand
{
	public Guid PostId { get; init; }
	public Guid CategoryId { get; init; }
}
