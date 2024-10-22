using ContentService.Core.Contracts.Aggregates.Posts.Queries.GetAll;
using ContentService.Core.Contracts.Aggregates.Posts.Queries.Models;
using ContentService.Core.Contracts.Aggregates.Posts.QueryRepositories;

using FluentResults;

using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;

namespace ContentService.Core.ApplicationService.Aggregates.Posts.QueriesHandlers;
public sealed class GetAllPostWithCommentsQueryHandler : IQueryHandler<GetAllPostWithCommentQuery, List<PostWithCommentsQueryDto>>
{
	private IPostQueryRepository _postQueryRepository;

	public GetAllPostWithCommentsQueryHandler(IPostQueryRepository postQueryRepository)
	{
		_postQueryRepository = postQueryRepository;
	}
	public async Task<Result<List<PostWithCommentsQueryDto>>> Handle(GetAllPostWithCommentQuery request, CancellationToken cancellationToken)
	{
		Result<List<PostWithCommentsQueryDto>> result = new();
		var posts = await _postQueryRepository.ExecuteAsync(request, cancellationToken);
		result.WithValue(posts);
		return result;
	}
}
