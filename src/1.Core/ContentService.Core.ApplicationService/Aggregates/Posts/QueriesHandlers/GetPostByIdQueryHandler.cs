using ContentService.Core.Contracts.Aggregates.Posts.Queries.GetPostById;
using ContentService.Core.Contracts.Aggregates.Posts.Queries.Models;
using ContentService.Core.Contracts.Aggregates.Posts.QueryRepositories;

using FluentResults;

using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;
using MDF.Resources.Common;
using MDF.Resources.Common.FormattedMessages;

namespace ContentService.Core.ApplicationService.Aggregates.Posts.QueriesHandlers;
public sealed class GetPostByIdQueryHandler : IQueryHandler<GetPostByIdQuery, PostQueryDto>
{
	IPostQueryRepository _postQueryRepository;

	public GetPostByIdQueryHandler(IPostQueryRepository postQueryRepository)
	{
		_postQueryRepository = postQueryRepository;
	}
	public async Task<Result<PostQueryDto>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
	{
		var result = new Result<PostQueryDto>();
		var postFound = await _postQueryRepository.ExecuteAsync(request);
		if (postFound is null)
		{
			result.WithError(ErrorMessages.NotFound(DataDictionary.Post + "  id: " + request.PostId));
			return result;
		}
		result.WithValue(postFound);
		return result;
	}
}
