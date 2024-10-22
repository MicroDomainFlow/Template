using ContentService.Core.Domain.Aggregates.Posts;
using ContentService.Core.Domain.Aggregates.Posts.Entities;
using ContentService.Core.Domain.Aggregates.Posts.ValueObjects;

using MDF.Framework.SeedWork.SharedKernel;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContentService.Infrastructure.Persistence.Sql.Commands.Aggregates.Posts.Configurations;
internal sealed class PostConfiguration : IEntityTypeConfiguration<Post>
{
	public void Configure(EntityTypeBuilder<Post> builder)
	{
		builder.Property(p => p.CategoryIds)
			.HasConversion(
				v => string.Join(',', v.Select(c => c.Value)),
				v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(c => GuidId.Create(c).Value).ToList()
			);

		builder.Property(p => p.Title)
			.IsRequired(true)
			.HasMaxLength(Title.Maximum)
			.HasConversion(p => p.Value, p => Title.Create(p).Value);

		builder.Property(p => p.Description)
			.IsRequired(true)
			.HasMaxLength(Description.Maximum)
			.HasConversion(d => d.Value, d => Description.Create(d).Value);

		builder.Property(p => p.Text)
			.IsRequired(true)
			.HasConversion(t => t.Value, t => Text.Create(t).Value);


		builder.OwnsMany<Comment>(c => c.Comments, cc =>
		{
			cc.ToTable("Comments");
			cc.Property(c => c.Email)
				.IsRequired(true)
				.HasConversion(e => e.Value, e => Email.Create(e).Value);
			cc.Property(c => c.Name)
				.IsRequired(true)
				.HasMaxLength(DisplayName.Maximum)
				.HasConversion(e => e.Value, e => DisplayName.Create(e).Value);
			cc.Property(c => c.CommentText)
				.IsRequired(true)
				.HasMaxLength(CommentText.Maximum)
				.HasConversion(e => e.Value, e => CommentText.Create(e).Value);
		});
	}
}
