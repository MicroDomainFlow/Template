namespace ContentService.Endpoints.API.ViewModels.Posts;

public record AddCommentToPostCommandVm
{
	public required Guid PostId { get; init; }
	public required string DisplayName { get; init; }
	public required string Email { get; init; }
	public required string CommentText { get; init; }
}
