using ContentService.Core.Contracts.Aggregates.Posts.CommandRepositories;
using ContentService.Core.Contracts.Aggregates.Posts.Commands;

using FluentResults;

using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;
using MDF.Resources.Common;
using MDF.Resources.Common.FormattedMessages;

namespace ContentService.Core.ApplicationService.Aggregates.Posts.CommandHandlers;
public sealed class RemovePostCommandHandler : ICommandHandler<RemovePostCommand>
{
	private readonly IPostCommandRepository _postRepository;

	public RemovePostCommandHandler(IPostCommandRepository postRepository)
	{
		_postRepository = postRepository;
	}

	public async Task<Result> Handle(RemovePostCommand request, CancellationToken cancellationToken)
	{
		Result result = new();
		if (request.PostId == null)
		{
			result.WithError(ValidationMessages.Required(result.ToString()));
			return result;
		}
		var postExist = await _postRepository.ExistsAsync(p => p.Id == request.PostId, cancellationToken);
		if (postExist)
		{
			_postRepository.DeleteGraphBy(request.PostId);
			await _postRepository.CommitAsync(cancellationToken);
			result.WithSuccess(SuccessMessages.SuccessDelete(DataDictionary.Post));
			return result;
		}
		result.WithError(ErrorMessages.NotFound($"{DataDictionary.Post} - {DataDictionary.Id} - {request.PostId}"));
		return result;
	}
}
