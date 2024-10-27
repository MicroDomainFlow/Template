using ContentService.Core.Contracts.Aggregates.Categories.Commands;
using ContentService.Core.Contracts.Aggregates.Posts.CommandRepositories;

using FluentResults;

using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;
using MDF.Resources.Common.FormattedMessages;

using Microsoft.Extensions.Logging;

namespace ContentService.Core.ApplicationService.Aggregates.Posts.CommandHandlers;
public class ChangeCategoryCommandHandler : ICommandHandler<ChangeCategoryCommand, Guid>
{
	private readonly IPostCommandRepository _postCommandRepository;
	private readonly ILogger<ChangeCategoryCommandHandler> _logger;

	public ChangeCategoryCommandHandler(IPostCommandRepository postCommandRepository, ILogger<ChangeCategoryCommandHandler> logger)
	{
		_postCommandRepository = postCommandRepository;
		_logger = logger;
	}

	public async Task<Result<Guid>> Handle(ChangeCategoryCommand request, CancellationToken cancellationToken)
	{
		var post = await _postCommandRepository.GetByAsync(request.PostId, cancellationToken);
		if (post is null)
		{
			return Result.Fail(ErrorMessages.NotFound(request.ToString()));
		}
		if (post.CategoryIds.Count < 0)
		{
			return Result.Fail(ErrorMessages.NotFound(request.OldCategoryId.ToString()));
		}
		post.ChangeCategory(request.OldCategoryId, request.NewCategoryId);
		if (post.Result.IsSuccess)
		{
			_postCommandRepository.UpdateBy(post);
			await _postCommandRepository.CommitAsync(cancellationToken);
			return post.Id;
		}
		post.Result.WithError(ErrorMessages.UnexpectedError);
		return post.Result;
	}
}
