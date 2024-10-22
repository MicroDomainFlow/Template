using ContentService.Core.Contracts.Aggregates.Posts.CommandRepositories;
using ContentService.Core.Contracts.Aggregates.Posts.Commands;
using ContentService.Core.Domain.Aggregates.Posts;

using FluentResults;

using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;
using MDF.Resources.Common.Messages;

namespace ContentService.Core.ApplicationService.Aggregates.Posts.CommandHandlers;
/// <summary>
/// repository یا Unitofwork باید در اینجا تزریق شوند
/// اگر چند ریپازیتوری باشد باید unitofwork تزریق شود
/// Iloger باید اینجا تزریق شود
/// </summary>
public sealed class CreatePostCommandHandler : ICommandHandler<CreatePostCommand, Guid>
{
	private readonly IPostCommandRepository _postRepository;
	public CreatePostCommandHandler(IPostCommandRepository postRepository)
	{
		_postRepository = postRepository;
	}

	public async Task<Result<Guid>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
	{
		var post = new Post()
			.Create(request.Title, request.Description, request.Text);

		if (post.Result.IsSuccess)
		{
			await _postRepository.InsertByAsync(post, cancellationToken);
			await _postRepository.CommitAsync(cancellationToken);
			return post.Id;
		}
		post.Result.WithError(Errors.UnexpectedError);
		return post.Result;
	}
}
