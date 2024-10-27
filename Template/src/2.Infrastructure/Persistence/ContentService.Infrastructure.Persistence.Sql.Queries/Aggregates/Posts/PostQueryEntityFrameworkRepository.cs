using AutoMapper;

using ContentService.Core.Contracts.Aggregates.Posts.Queries.GetAll;
using ContentService.Core.Contracts.Aggregates.Posts.Queries.GetPostAndCommentById;
using ContentService.Core.Contracts.Aggregates.Posts.Queries.GetPostById;
using ContentService.Core.Contracts.Aggregates.Posts.Queries.Models;
using ContentService.Core.Contracts.Aggregates.Posts.QueryRepositories;
using ContentService.Infrastructure.Persistence.Sql.Queries.Common;

using MDF.Framework.Infrastructure.Queries;

using Microsoft.EntityFrameworkCore;

namespace ContentService.Infrastructure.Persistence.Sql.Queries.Aggregates.Posts;
public class PostQueryEntityFrameworkRepository : BaseQueryRepository<ContentServiceQueryDbContext>, IPostQueryRepository
{
	private IMapper _mapper;
	public PostQueryEntityFrameworkRepository(ContentServiceQueryDbContext dbContext, IMapper mapper) : base(dbContext)
	{
		_mapper = mapper;
	}

	public Task<PostQueryDto> ExecuteAsync(GetPostByIdQuery query, CancellationToken cancellationToken = default)
	{
		//manual mapping
		return DbContext.Posts.Select(c => new PostQueryDto()
		{
			Id = c.Id,
			CategoryIds = c.CategoryIds,
			Title = c.Title,
			Description = c.Description,
			Text = c.Text
		}).FirstOrDefaultAsync(c => c.Id.Equals(query.PostId), cancellationToken);
	}

	public Task<List<PostQueryDto>> ExecuteAsync(GetAllPostQuery query, CancellationToken cancellationToken = default)
	{
		//manual mapping
		return DbContext.Posts.Select(c => new PostQueryDto()
		{
			Id = c.Id,
			CategoryIds = c.CategoryIds,
			Title = c.Title,
			Description = c.Description,
			Text = c.Text
		}).ToListAsync(cancellationToken);
	}

	public async Task<List<PostWithCommentsQueryDto>> ExecuteAsync(GetAllPostWithCommentQuery query, CancellationToken cancellationToken = default)
	{
		var posts = await DbContext.Posts.Include(p => p.Comments).ToListAsync(cancellationToken);
		var result = _mapper.Map<List<PostWithCommentsQueryDto>>(posts);
		return result;
	}

	public async Task<PostWithCommentsQueryDto> ExecuteAsync(GetPostWithCommentsByIdQuery query, CancellationToken cancellationToken = default)
	{
		var post = await DbContext.Posts.Include(p => p.Comments).FirstOrDefaultAsync(p => p.Id == query.PostId, cancellationToken);
		if (post is not null)
		{
			var postQuery = _mapper.Map<PostWithCommentsQueryDto>(post);

			return postQuery;
		}
		return new PostWithCommentsQueryDto() { Comments = null, Description = null, Text = null, Title = null, Id = new Guid() };
	}
}
