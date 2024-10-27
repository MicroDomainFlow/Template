namespace ContentService.Endpoints.API.ViewModels.Categories;

public class AddParentCategoryCommandVm
{
	public required Guid ParentCategoryId { get; init; }
	public required Guid Id { get; init; }
}
