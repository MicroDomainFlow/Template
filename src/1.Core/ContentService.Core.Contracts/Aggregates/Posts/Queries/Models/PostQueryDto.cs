namespace ContentService.Core.Contracts.Aggregates.Posts.Queries.Models;
public record PostQueryDto
{
	public Guid Id { get; init; }
	public List<Guid?> CategoryIds { get; init; }
	public required string Title { get; init; }
	public required string Description { get; init; }
	public required string Text { get; init; }

}