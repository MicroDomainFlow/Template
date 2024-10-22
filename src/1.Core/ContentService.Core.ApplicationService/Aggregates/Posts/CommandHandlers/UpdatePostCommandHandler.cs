using ContentService.Core.Contracts.Aggregates.Posts.CommandRepositories;
using ContentService.Core.Contracts.Aggregates.Posts.Commands;

using FluentResults;

using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;
using MDF.Resources.Common.FormattedMessages;

namespace ContentService.Core.ApplicationService.Aggregates.Posts.CommandHandlers;
public sealed class UpdatePostCommandHandler : ICommandHandler<UpdatePostCommand, Guid>
{
	private readonly IPostCommandRepository _postCommandRepository;

	public UpdatePostCommandHandler(IPostCommandRepository postCommandRepository)
	{
		_postCommandRepository = postCommandRepository;
	}

	public async Task<Result<Guid>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
	{
		var post = await _postCommandRepository.GetByAsync(request.PostId, cancellationToken);
		if (post is null)
		{
			return Result.Fail(ErrorMessages.NotFound(request.ToString()));
		}
		post.UpdatePost(request.Title, request.Description, request.Text);
		if (post.Result.IsFailed)
		{
			return post.Result;
		}
		_postCommandRepository.UpdateBy(post);
		await _postCommandRepository.CommitAsync(cancellationToken);
		return post.Id;
	}
}
