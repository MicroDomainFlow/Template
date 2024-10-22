namespace ContentService.Endpoints.API.ViewModels.Posts;

public readonly record struct AddCategoryVm
{
	public Guid PostId { get; init; }
	public Guid CategoryId { get; init; }
}