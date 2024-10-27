using AutoMapper;

using ContentService.Core.Contracts.Aggregates.Categories.Commands;
using ContentService.Core.Contracts.Aggregates.Categories.Queries.GetAll;
using ContentService.Core.Contracts.Aggregates.Categories.Queries.Models;
using ContentService.Endpoints.API.ViewModels.Categories;

namespace ContentService.Endpoints.API.ProfilesViewModel;

public class CategoryProfile : Profile
{
	public CategoryProfile()
	{
		CreateMap<CreateCategoryCommand, CreateCategoryCommandVm>().ReverseMap()
			.ForMember(ds => ds.Title, opt => opt.MapFrom(src => src.CategoryTitle))
			;
		CreateMap<AddParentCategoryCommand, AddParentCategoryCommandVm>().ReverseMap();
		CreateMap<GetAllSubCategoryQuery, List<CategoryQueryDto>>().ReverseMap();
		CreateMap<GetAllCategoryQuery, List<CategoryQueryDto>>().ReverseMap();
	}
}