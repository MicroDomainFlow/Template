
using ContentService.Core.Domain.Aggregates.Posts.ValueObjects;
using ContentService.Infrastructure.Persistence.Sql.Queries.QueryModels;

using MDF.Framework.SeedWork.SharedKernel;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContentService.Infrastructure.Persistence.Sql.Queries.Aggregates.Posts.Configurations;
internal sealed class PostQueryConfiguration : IEntityTypeConfiguration<PostQuery>
{
	public void Configure(EntityTypeBuilder<PostQuery> builder)
	{
		builder.ToTable("Posts");
		builder.Property(e => e.CategoryIds)
			.HasConversion(
				v => string.Join(',', v),
				v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(c => (Guid?)GuidId.Create(c).Value).ToList());

		builder.Property(p => p.Title)
			.IsRequired(true)
			.HasMaxLength(Title.Maximum);

		builder.Property(p => p.Description)
			.IsRequired(true)
			.HasMaxLength(Description.Maximum);

		builder.Property(p => p.Text)
			.IsRequired(true);


		builder.OwnsMany<CommentQuery>(c => c.Comments, cc =>
		{
			cc.ToTable("Comments");
			cc.Property(c => c.Email)
				.IsRequired(true);
			cc.Property(c => c.Name)
				.IsRequired(true)
				.HasMaxLength(DisplayName.Maximum);
			cc.Property(c => c.CommentText)
				.IsRequired(true)
				.HasMaxLength(CommentText.Maximum);
		});
	}
}
