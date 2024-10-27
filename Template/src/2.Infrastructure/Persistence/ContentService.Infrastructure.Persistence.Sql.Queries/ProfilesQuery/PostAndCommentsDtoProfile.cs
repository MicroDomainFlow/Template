using AutoMapper;

using ContentService.Core.Contracts.Aggregates.Posts.Queries.GetPostAndCommentById;
using ContentService.Core.Contracts.Aggregates.Posts.Queries.Models;
using ContentService.Infrastructure.Persistence.Sql.Queries.QueryModels;


namespace ContentService.Infrastructure.Persistence.Sql.Queries.ProfilesQuery;
public class PostAndCommentsDtoProfile : Profile
{
	public PostAndCommentsDtoProfile()
	{
		CreateMap<PostQuery, PostWithCommentsQueryDto>()
			.ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments.Select(c => new QueryCommentDto()
			{
				Id = c.Id,
				CommentText = c.CommentText,
				Email = c.Email,
				Name = c.Name,
				PostId = c.Post.Id
			})))
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)) // نگاشت سایر خصوصیات
			.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
			.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
			.ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
			.ReverseMap();

		//CreateMap<CommentDto, Comment>()
		//	.ForMember(dest => dest.Post, opt => opt.Ignore()) // Ignore mapping Post property (optional)
		//	.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
		//	.ForMember(dest => dest.CommentText, opt => opt.MapFrom(src => src.CommentText))
		//	.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
		//	.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
		//	.ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId))
		//	.ReverseMap();
	}
}
