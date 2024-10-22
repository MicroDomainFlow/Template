using AutoMapper;

using ContentService.Core.Contracts.Aggregates.Categories.Commands;
using ContentService.Core.Contracts.Aggregates.Posts.Commands;
using ContentService.Core.Contracts.Aggregates.Posts.Commands.Comment;
using ContentService.Core.Contracts.Aggregates.Posts.Queries.GetAll;
using ContentService.Core.Contracts.Aggregates.Posts.Queries.GetPostAndCommentById;
using ContentService.Core.Contracts.Aggregates.Posts.Queries.GetPostById;
using ContentService.Core.Contracts.Aggregates.Posts.Queries.Models;
using ContentService.Endpoints.API.ViewModels.Posts;

using MDF.Framework.Endpoints.Api;
using MDF.Framework.Extensions.Results;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace ContentService.Endpoints.API.Controllers;
[Route("api/[controller]/[action]")]
public class PostController : BaseController
{
	public PostController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
	{
	}

	[ProducesResponseType(type: typeof(CustomResult<List<PostQueryDto>>), statusCode: StatusCodes.Status200OK)]
	[HttpGet("")]
	public Task<IActionResult> GetAllPostAsync()
	{
		return QueryAsync<GetAllPostQuery, List<PostQueryDto>>(new GetAllPostQuery());
	}
	[ProducesResponseType(type: typeof(CustomResult<List<PostWithCommentsQueryDto>>), statusCode: StatusCodes.Status200OK)]
	[HttpGet("")]
	public Task<IActionResult> GetAllPostWithCommentAsync()
	{
		return QueryAsync<GetAllPostWithCommentQuery, List<PostWithCommentsQueryDto>>(new GetAllPostWithCommentQuery());
	}

	[ProducesResponseType(type: typeof(CustomResult<PostQueryDto>), statusCode: StatusCodes.Status200OK)]
	[ProducesResponseType(type: typeof(CustomResult), statusCode: StatusCodes.Status400BadRequest)]
	[HttpGet("")]
	public Task<IActionResult> GetPostAsync(GetPostByIdQuery id)
	{
		return QueryAsync<GetPostByIdQuery, PostQueryDto>(id);
	}

	[ProducesResponseType(type: typeof(CustomResult<PostQueryDto>), statusCode: StatusCodes.Status200OK)]
	[ProducesResponseType(type: typeof(CustomResult), statusCode: StatusCodes.Status400BadRequest)]
	[HttpGet("")]
	public Task<IActionResult> GetPostAndCommentsAsync(GetPostWithCommentsByIdQuery id)
	{
		return QueryAsync<GetPostWithCommentsByIdQuery, PostWithCommentsQueryDto>(id);
	}

	[ProducesResponseType(type: typeof(CustomResult<Guid>), statusCode: StatusCodes.Status200OK)]
	[ProducesResponseType(type: typeof(CustomResult), statusCode: StatusCodes.Status400BadRequest)]
	[HttpPost("")]
	public Task<IActionResult> CreatePostAsync([FromBody] CreatePostVm createPostVm)
	{
		var postCommand = Mapper.Map<CreatePostCommand>(createPostVm);
		return CreateAsync<CreatePostCommand, Guid>(postCommand);
	}
	[ProducesResponseType(type: typeof(CustomResult<Guid>), statusCode: StatusCodes.Status200OK)]
	[ProducesResponseType(type: typeof(CustomResult), statusCode: StatusCodes.Status400BadRequest)]
	[HttpPut("")]
	public Task<IActionResult> AddCategoryAsync([FromBody] AddCategoryVm addCategoryVm)
	{
		var addCategoryCommand = Mapper.Map<AddCategoryCommand>(addCategoryVm);
		return EditAsync<AddCategoryCommand, Guid>(addCategoryCommand);
	}

	[ProducesResponseType(type: typeof(CustomResult<Guid>), statusCode: StatusCodes.Status200OK)]
	[ProducesResponseType(type: typeof(CustomResult), statusCode: StatusCodes.Status400BadRequest)]
	[HttpPatch("")]
	public Task<IActionResult> ChangeCategoryAsync([FromBody] ChangeCategoryVm changeCategoryVm)
	{
		var changeCategoryCommand = Mapper.Map<ChangeCategoryCommand>(changeCategoryVm);
		return EditAsync<ChangeCategoryCommand, Guid>(changeCategoryCommand);
	}

	[ProducesResponseType(type: typeof(CustomResult<Guid>), statusCode: StatusCodes.Status200OK)]
	[ProducesResponseType(type: typeof(CustomResult), statusCode: StatusCodes.Status400BadRequest)]
	[HttpDelete("")]
	public Task<IActionResult> RemovePostCategoryAsync([FromBody] RemovePostCategoryVm removePostCategoryVm)
	{
		var removePostCategoryCommand = Mapper.Map<RemovePostCategoryCommand>(removePostCategoryVm);
		return DeleteAsync<RemovePostCategoryCommand>(removePostCategoryCommand);
	}

	[ProducesResponseType(type: typeof(CustomResult<Guid>), statusCode: StatusCodes.Status200OK)]
	[ProducesResponseType(type: typeof(CustomResult), statusCode: StatusCodes.Status400BadRequest)]
	[HttpPut("")]
	public Task<IActionResult> UpdatePostAsync([FromBody] UpdatePostCommand updatePostCommand)
	{
		//todo mapper her
		return EditAsync<UpdatePostCommand, Guid>(updatePostCommand);
	}

	[ProducesResponseType(type: typeof(CustomResult), statusCode: StatusCodes.Status200OK)]
	[ProducesResponseType(type: typeof(CustomResult), statusCode: StatusCodes.Status400BadRequest)]
	[HttpDelete("")]
	public Task<IActionResult> DeletePostAsync([FromBody] RemovePostCommand removePostCommand)
	{
		//todo mapper her
		return DeleteAsync<RemovePostCommand>(removePostCommand);
	}
	#region Comment
	[ProducesResponseType(type: typeof(CustomResult<Guid>), statusCode: StatusCodes.Status200OK)]
	[ProducesResponseType(type: typeof(CustomResult), statusCode: StatusCodes.Status400BadRequest)]
	[HttpPost("")]
	public Task<IActionResult> AddCommentToThePostAsync([FromBody] AddCommentToPostCommandVm addCommentToPostCommandVm)
	{
		var commentToPostCommand = Mapper.Map<AddCommentToPostCommand>(addCommentToPostCommandVm);
		return CreateAsync<AddCommentToPostCommand, Guid>(commentToPostCommand);
	}

	[ProducesResponseType(type: typeof(CustomResult<Guid>), statusCode: StatusCodes.Status200OK)]
	[ProducesResponseType(type: typeof(CustomResult), statusCode: StatusCodes.Status400BadRequest)]
	[HttpPut("")]
	public Task<IActionResult> EditCommentAsync([FromBody] EditCommentThePostCommand editCommentThePostCommand)
	{
		return EditAsync<EditCommentThePostCommand, Guid>(editCommentThePostCommand);
	}

	[ProducesResponseType(type: typeof(CustomResult), statusCode: StatusCodes.Status200OK)]
	[ProducesResponseType(type: typeof(CustomResult), statusCode: StatusCodes.Status400BadRequest)]
	[HttpDelete("")]
	public Task<IActionResult> DeleteCommentAsync([FromBody] RemoveCommentFromPostCommand removeCommentFromPostCommand)
	{
		return DeleteAsync<RemoveCommentFromPostCommand>(removeCommentFromPostCommand);
	}
	#endregion
}
