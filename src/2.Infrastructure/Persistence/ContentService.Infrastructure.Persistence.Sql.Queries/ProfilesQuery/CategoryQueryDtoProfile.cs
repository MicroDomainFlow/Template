using AutoMapper;

using ContentService.Core.Contracts.Aggregates.Categories.Queries.Models;
using ContentService.Infrastructure.Persistence.Sql.Queries.Aggregates.Categories.QueryModels;

namespace ContentService.Infrastructure.Persistence.Sql.Queries.ProfilesQuery;
public class CategoryQueryDtoProfile : Profile
{
	public CategoryQueryDtoProfile()
	{
		CreateMap<CategoryQuery, CategoryQueryDto>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.CategoryTitle, opt => opt.MapFrom(src => src.CategoryTitle))
			.ForMember(dest => dest.PostIds, opt => opt.MapFrom(src => src.PostIds))
			.ForMember(dest => dest.ParentCategoriesId, opt => opt.MapFrom(src => src.ParentCategoriesId))
			.ReverseMap();
	}
}
