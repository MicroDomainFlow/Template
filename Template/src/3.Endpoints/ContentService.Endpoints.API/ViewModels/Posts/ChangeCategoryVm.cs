namespace ContentService.Endpoints.API.ViewModels.Posts;

public readonly record struct ChangeCategoryVm
{
	public Guid PostId { get; init; }
	public Guid OldCategoryId { get; init; }
	public Guid NewCategoryId { get; init; }
}