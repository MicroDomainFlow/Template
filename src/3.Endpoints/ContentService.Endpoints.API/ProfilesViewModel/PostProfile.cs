using AutoMapper;

using ContentService.Core.Contracts.Aggregates.Categories.Commands;
using ContentService.Core.Contracts.Aggregates.Posts.Commands;
using ContentService.Core.Contracts.Aggregates.Posts.Commands.Comment;
using ContentService.Endpoints.API.ViewModels.Posts;

namespace ContentService.Endpoints.API.ProfilesViewModel;

public class PostProfile : Profile
{
	public PostProfile()
	{
		CreateMap<CreatePostCommand, CreatePostVm>().ReverseMap();
		CreateMap<AddCommentToPostCommand, AddCommentToPostCommandVm>().ReverseMap();
		CreateMap<AddCategoryCommand, AddCategoryVm>().ReverseMap();
		CreateMap<ChangeCategoryCommand, ChangeCategoryVm>().ReverseMap();
		CreateMap<RemovePostCategoryCommand, RemovePostCategoryVm>().ReverseMap();
	}
}
