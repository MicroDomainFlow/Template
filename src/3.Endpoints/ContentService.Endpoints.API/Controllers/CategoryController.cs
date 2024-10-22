using AutoMapper;

using ContentService.Core.Contracts.Aggregates.Categories.Commands;
using ContentService.Core.Contracts.Aggregates.Categories.Queries.GetAll;
using ContentService.Core.Contracts.Aggregates.Categories.Queries.GetCategoryById;
using ContentService.Core.Contracts.Aggregates.Categories.Queries.Models;
using ContentService.Endpoints.API.ViewModels.Categories;

using MDF.Framework.Endpoints.Api;
using MDF.Framework.Extensions.Results;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace ContentService.Endpoints.API.Controllers;
[Route("api/[controller]/[action]")]
public class CategoryController : BaseController
{
	public CategoryController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
	{
	}

	[ProducesResponseType(type: typeof(CustomResult<List<CategoryQueryDto>>), statusCode: StatusCodes.Status200OK)]
	[HttpGet("")]
	public Task<IActionResult> GetAllCategoryAsync()
	{
		return QueryAsync<GetAllCategoryQuery, List<CategoryQueryDto>>(new GetAllCategoryQuery());
	}

	[ProducesResponseType(type: typeof(CustomResult<CategoryQueryDto>), statusCode: StatusCodes.Status200OK)]
	[ProducesResponseType(type: typeof(CustomResult), statusCode: StatusCodes.Status400BadRequest)]
	[HttpGet("")]
	public Task<IActionResult> GetCategoryAsync(GetCategoryByIdQuery categoryId)
	{
		return QueryAsync<GetCategoryByIdQuery, CategoryQueryDto>(categoryId);
	}

	[ProducesResponseType(type: typeof(CustomResult<List<CategoryQueryDto>>), statusCode: StatusCodes.Status200OK)]
	[ProducesResponseType(type: typeof(CustomResult), statusCode: StatusCodes.Status400BadRequest)]
	[HttpGet("")]
	public Task<IActionResult> GetSubCategoriesAsync(GetAllSubCategoryQuery id)
	{
		return QueryAsync<GetAllSubCategoryQuery, List<CategoryQueryDto>>(id);
	}

	[ProducesResponseType(type: typeof(CustomResult<Guid>), statusCode: StatusCodes.Status200OK)]
	[ProducesResponseType(type: typeof(CustomResult), statusCode: StatusCodes.Status400BadRequest)]
	[HttpPost("")]
	public Task<IActionResult> CreateCategoryAsync([FromBody] CreateCategoryCommandVm createCategoryCommandVm)
	{
		var categoryCommand = Mapper.Map<CreateCategoryCommand>(createCategoryCommandVm);
		return CreateAsync<CreateCategoryCommand, Guid>(categoryCommand);
	}

	[ProducesResponseType(type: typeof(CustomResult<Guid>), statusCode: StatusCodes.Status200OK)]
	[ProducesResponseType(type: typeof(CustomResult), statusCode: StatusCodes.Status400BadRequest)]
	[HttpPut("")]
	public Task<IActionResult> AddParentCategoryAsync([FromBody] AddParentCategoryCommandVm addParentCategoryCommandVm)
	{
		var addParentCategoryCommand = Mapper.Map<AddParentCategoryCommand>(addParentCategoryCommandVm);
		return CreateAsync<AddParentCategoryCommand, Guid>(addParentCategoryCommand);
	}

	[ProducesResponseType(type: typeof(CustomResult<Guid>), statusCode: StatusCodes.Status200OK)]
	[ProducesResponseType(type: typeof(CustomResult), statusCode: StatusCodes.Status400BadRequest)]
	[HttpPut("")]
	public Task<IActionResult> CategoryTitleChangeAsync([FromBody] CategoryTitleChangeCommand categoryTitleChangeCommand)
	{
		// mapper her if needed
		return EditAsync<CategoryTitleChangeCommand, Guid>(categoryTitleChangeCommand);
	}

	[ProducesResponseType(type: typeof(CustomResult), statusCode: StatusCodes.Status200OK)]
	[ProducesResponseType(type: typeof(CustomResult), statusCode: StatusCodes.Status400BadRequest)]
	[HttpDelete("")]
	public Task<IActionResult> DeleteCategoryAsync([FromBody] RemoveCategoryCommand removeCategoryCommand)
	{
		// mapper her if needed
		return DeleteAsync<RemoveCategoryCommand>(removeCategoryCommand);
	}

	[ProducesResponseType(type: typeof(CustomResult), statusCode: StatusCodes.Status200OK)]
	[ProducesResponseType(type: typeof(CustomResult), statusCode: StatusCodes.Status400BadRequest)]
	[HttpDelete("")]
	public Task<IActionResult> DeleteParentCategoryAsync([FromBody] RemoveParentCategoryCommand removeCategoryCommand)
	{
		// mapper her if needed
		return DeleteAsync<RemoveParentCategoryCommand>(removeCategoryCommand);
	}

}
