using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;

namespace ContentService.Core.Contracts.Aggregates.Posts.Commands.Comment;
public record AddCommentToPostCommand : ICommand<Guid>
{
	public required Guid CommandId { get; init; }
	public required Guid PostId { get; init; }
	public required string DisplayName { get; init; }
	public required string Email { get; init; }
	public required string CommentText { get; init; }
}
