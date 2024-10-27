using ContentService.Core.Contracts.Aggregates.Posts.Queries.Models;

using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;

namespace ContentService.Core.Contracts.Aggregates.Posts.Queries.GetPostById;
public record GetPostByIdQuery : IQuery<PostQueryDto>
{
	public Guid PostId { get; init; }
}
