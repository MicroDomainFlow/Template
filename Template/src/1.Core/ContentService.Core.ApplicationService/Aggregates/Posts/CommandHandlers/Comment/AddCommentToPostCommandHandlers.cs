using ContentService.Core.Contracts.Aggregates.Posts.CommandRepositories;
using ContentService.Core.Contracts.Aggregates.Posts.Commands.Comment;

using FluentResults;

using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;
using MDF.Resources.Common.FormattedMessages;

namespace ContentService.Core.ApplicationService.Aggregates.Posts.CommandHandlers.Comment;
public sealed class AddCommentToPostCommandHandlers : ICommandHandler<AddCommentToPostCommand, Guid>
{
	private readonly IPostCommandRepository _postRepository;

	public AddCommentToPostCommandHandlers(IPostCommandRepository postRepository)
	{
		_postRepository = postRepository;
	}

	public async Task<Result<Guid>> Handle(AddCommentToPostCommand request, CancellationToken cancellationToken)
	{
		var post = await _postRepository.GetGraphByAsync(request.PostId, cancellationToken);

		if (post is not null)
		{
			post.AddComment(request.DisplayName, request.Email, request.CommentText);

			if (post.Result.IsSuccess)
			{
				_postRepository.UpdateBy(post);
				await _postRepository.CommitAsync(cancellationToken);
				return post.Id;
			}
			return post.Result;
		}
		return Result.Fail(ErrorMessages.NotFound(request.ToString()));
	}

}
