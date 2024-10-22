using ContentService.Core.Contracts.Aggregates.Posts.CommandRepositories;
using ContentService.Core.Contracts.Aggregates.Posts.Commands.Comment;

using FluentResults;

using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;
using MDF.Resources.Common.FormattedMessages;

namespace ContentService.Core.ApplicationService.Aggregates.Posts.CommandHandlers.Comment;
public class EditCommentThePostCommandHandler : ICommandHandler<EditCommentThePostCommand, Guid>
{
	private readonly IPostCommandRepository _postCommandRepository;

	public EditCommentThePostCommandHandler(IPostCommandRepository postCommandRepository)
	{
		_postCommandRepository = postCommandRepository;
	}
	public async Task<Result<Guid>> Handle(EditCommentThePostCommand request, CancellationToken cancellationToken)
	{
		var post = await _postCommandRepository.GetGraphByAsync(request.PostId, cancellationToken);

		if (post is not null)
		{
			post.ChangeCommentText(request.DisplayName, request.Email, request.CommentText, request.CommentNewText);

			if (post.Result.IsSuccess)
			{
				_postCommandRepository.UpdateBy(post);
				await _postCommandRepository.CommitAsync(cancellationToken);
				return post.Id;
			}
			return post.Result;
		}
		var notFoundValue = new Result<Guid>();
		//notFoundValue.WithError(ValidationHelperMessages.NotFound(DataDictionary.Post));
		notFoundValue.WithError(ErrorMessages.NotFound(request.ToString()));
		return notFoundValue;
	}
}
