namespace ContentService.Endpoints.API.ViewModels.Categories;

public class CategoryQueryVm
{
	public Guid Id { get; init; }
	public string Title { get; init; }
	public Guid? ParentId { get; init; }
}