using ContentService.Core.Domain.Aggregates.Categories;
using ContentService.Core.Domain.Aggregates.Categories.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContentService.Infrastructure.Persistence.Sql.Queries.Aggregates.Categories.Configurations;
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
	public void Configure(EntityTypeBuilder<Category> builder)
	{
		//builder.Property(c => c.CategoryTitle)
		//	.IsRequired(true)
		//	.HasMaxLength(CategoryTitle.Maximum)
		//	.HasConversion(t => t.Value, t => CategoryTitle.Create(t).Value);

		//builder.Property(c => c.PostIds)
		//	.HasConversion(
		//		v => string.Join(',', v),
		//		v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList());

		//builder.Property(c => c.ParentCategoriesId)
		//	.HasConversion(
		//		v => string.Join(',', v),
		//		v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList<Guid>());


		builder.ToTable("Categories");
		builder.Property(e => e.Id).ValueGeneratedNever();
		builder.Property(e => e.CategoryTitle)
					  .IsRequired()
					  .HasMaxLength(CategoryTitle.Maximum)
		.HasConversion(t => t.Value, t => CategoryTitle.Create(t).Value);
		builder.Property(e => e.ParentCategoriesId)
					  .HasConversion(
						  v => string.Join(',', v),
						  v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList<Guid>())
					  .Metadata.SetValueComparer(new ValueComparer<List<Guid>>(
			(c1, c2) => c1.SequenceEqual(c2),
			c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
			c => c.ToList()));
		builder.Property(e => e.PostIds)
					  .HasConversion(
						  v => string.Join(',', v),
						  v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList())
			.Metadata.SetValueComparer(new ValueComparer<List<Guid>>(
				(c1, c2) => c1.SequenceEqual(c2),
				c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
				c => c.ToList()));


	}
}
