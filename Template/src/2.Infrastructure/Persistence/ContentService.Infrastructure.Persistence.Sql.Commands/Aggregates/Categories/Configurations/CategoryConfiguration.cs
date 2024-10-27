using ContentService.Core.Domain.Aggregates.Categories;
using ContentService.Core.Domain.Aggregates.Categories.ValueObjects;
using ContentService.Core.Domain.Aggregates.Posts.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContentService.Infrastructure.Persistence.Sql.Commands.Aggregates.Categories.Configurations;
internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
	public void Configure(EntityTypeBuilder<Category> builder)
	{
		builder.Property(c => c.CategoryTitle)
			.IsRequired(true)
			.HasMaxLength(Title.Maximum)
			.HasConversion(t => t.Value, t => CategoryTitle.Create(t).Value);

		builder.Property(c => c.PostIds)
.HasConversion(
	v => string.Join(',', v),
	v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList());

		builder.Property(c => c.ParentCategoriesId)
			.HasConversion(
				v => string.Join(',', v),
				v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList());
	}
}
