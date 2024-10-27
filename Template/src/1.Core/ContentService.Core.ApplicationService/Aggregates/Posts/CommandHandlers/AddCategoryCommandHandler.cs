using ContentService.Core.Contracts.Aggregates.Categories.CommandRepositories;
using ContentService.Core.Contracts.Aggregates.Posts.CommandRepositories;
using ContentService.Core.Contracts.Aggregates.Posts.Commands;

using FluentResults;

using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;
using MDF.Framework.LayersContracts.Persistence.Commands;
using MDF.Resources.Common.FormattedMessages;

using Microsoft.Extensions.Logging;

namespace ContentService.Core.ApplicationService.Aggregates.Posts.CommandHandlers;
public class AddCategoryCommandHandler : ICommandHandler<AddCategoryCommand, Guid>
{
	private readonly IPostCommandRepository _postRepository;
	private readonly ICategoryCommandRepository _categoryRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<AddCategoryCommandHandler> _logger;

	public AddCategoryCommandHandler(IPostCommandRepository postRepository, ILogger<AddCategoryCommandHandler> logger, ICategoryCommandRepository categoryRepository, IUnitOfWork unitOfWork)
	{
		_postRepository = postRepository;
		_logger = logger;
		_categoryRepository = categoryRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<Guid>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
	{
		var category = await _categoryRepository.GetByAsync(request.CategoryId, cancellationToken);
		var post = await _postRepository.GetByAsync(request.PostId, cancellationToken);

		if (post is null)
		{
			return Result.Fail<Guid>(ErrorMessages.NotFound(request.ToString()));
		}
		if (category is null)
		{
			return Result.Fail<Guid>(ErrorMessages.NotFound(request.ToString()));
		}

		post.AddCategory(request.CategoryId);
		category.AddPostId(request.PostId);

		if (post.Result.IsSuccess && category.Result.IsSuccess)
		{

			_postRepository.UpdateBy(post);
			_categoryRepository.UpdateBy(category);
			await _unitOfWork.CommitAsync(cancellationToken);
			return post.Id;


		}

		post.Result.WithError(ErrorMessages.UnexpectedError);
		return post.Result;
	}

}
