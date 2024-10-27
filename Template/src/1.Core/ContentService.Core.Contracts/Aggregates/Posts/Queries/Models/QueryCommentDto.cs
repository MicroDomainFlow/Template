namespace ContentService.Core.Contracts.Aggregates.Posts.Queries.Models;
public class QueryCommentDto
{
    public Guid Id { get; set; }

    public Guid PostId { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string CommentText { get; set; }
}
