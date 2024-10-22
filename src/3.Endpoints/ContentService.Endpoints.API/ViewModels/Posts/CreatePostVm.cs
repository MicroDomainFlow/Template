namespace ContentService.Endpoints.API.ViewModels.Posts;

public class CreatePostVm
{
	public required string Title { get; init; }
	public required string Description { get; init; }
	public required string Text { get; init; }
}
