using ContentService.Core.Contracts.Aggregates.Posts.Queries.Models;

using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;

namespace ContentService.Core.Contracts.Aggregates.Posts.Queries.GetAll;
public record GetAllPostWithCommentQuery : IQuery<List<PostWithCommentsQueryDto>>
{
}
