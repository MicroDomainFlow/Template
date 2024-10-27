using ContentService.Core.Contracts.Aggregates.Posts.CommandRepositories;
using ContentService.Core.Contracts.Aggregates.Posts.Commands.Comment;

using FluentResults;

using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;
using MDF.Resources.Common.FormattedMessages;

using Microsoft.Extensions.Logging;

namespace ContentService.Core.ApplicationService.Aggregates.Posts.CommandHandlers.Comment;
public class RemoveCommentFromPostCommandHandler : ICommandHandler<RemoveCommentFromPostCommand>
{
	private readonly IPostCommandRepository _postCommandRepository;
	private readonly ILogger<RemoveCommentFromPostCommandHandler> _logger;
	public RemoveCommentFromPostCommandHandler(IPostCommandRepository postCommandRepository, ILogger<RemoveCommentFromPostCommandHandler> logger)
	{
		_postCommandRepository = postCommandRepository;
		_logger = logger;
	}
	public async Task<Result> Handle(RemoveCommentFromPostCommand request, CancellationToken cancellationToken)
	{
		var post = await _postCommandRepository.GetByAsync(request.PostId, cancellationToken);

		if (post is not null)
		{
			post.RemoveComment(request.Name, request.Email, request.Text);

			if (post.Result.IsSuccess)
			{
				_postCommandRepository.UpdateBy(post);
				await _postCommandRepository.CommitAsync(cancellationToken);

				post.Result.WithSuccess(SuccessMessages.SuccessDelete(request.ToString()));
				_logger.LogInformation(post.Result.Successes[0].Message);

				return post.Result;
			}
		}

		return Result.Fail(ErrorMessages.NotFound(request.ToString()));
	}
}
