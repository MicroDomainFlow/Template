namespace ContentService.Infrastructure.Persistence.Sql.Queries.Aggregates.Categories.QueryModels;
public class CategoryQuery
{
	public Guid Id { get; init; }
	public string? CategoryTitle { get; init; }
	public List<Guid>? ParentCategoriesId { get; init; }
	public List<Guid>? PostIds { get; init; }
}
