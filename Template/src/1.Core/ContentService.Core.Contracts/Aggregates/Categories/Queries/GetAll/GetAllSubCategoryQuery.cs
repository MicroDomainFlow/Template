using ContentService.Core.Contracts.Aggregates.Categories.Queries.Models;

using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions.CQRS;

namespace ContentService.Core.Contracts.Aggregates.Categories.Queries.GetAll;
public class GetAllSubCategoryQuery : IQuery<List<CategoryQueryDto>>
{
	public Guid CategoryId { get; set; }
}
