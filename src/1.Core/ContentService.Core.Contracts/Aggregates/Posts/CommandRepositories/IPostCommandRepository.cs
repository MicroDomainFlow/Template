using ContentService.Core.Domain.Aggregates.Posts;

using MDF.Framework.LayersContracts.Persistence.Commands;

namespace ContentService.Core.Contracts.Aggregates.Posts.CommandRepositories;

public interface IPostCommandRepository : ICommandRepository<Post>
{
	//اینجا باید متد های مورد نیاز را نوشت

}