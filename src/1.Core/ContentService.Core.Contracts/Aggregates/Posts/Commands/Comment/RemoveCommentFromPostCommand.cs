using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;

namespace ContentService.Core.Contracts.Aggregates.Posts.Commands.Comment;
public class RemoveCommentFromPostCommand(Guid postId, string name, string email, string text) : BaseCommand
{
	// or useing valueObject instead of string
	public Guid PostId { get; } = postId;
	public string Name { get; } = name;
	public string Email { get; } = email;
	public string Text { get; } = text;
}
