using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;

namespace ContentService.Core.Contracts.Aggregates.Posts.Commands.Comment;
public record EditCommentThePostCommand : ICommand<Guid>
{
	public required Guid PostId { get; set; }
	public required string DisplayName { get; set; }
	public required string Email { get; set; }
	public required string CommentText { get; set; }
	public required string CommentNewText { get; set; }
}
