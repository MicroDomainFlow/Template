﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
#if (IncludeExample)
using ContentService.Infrastructure.Persistence.Sql.Queries.Aggregates.Categories.QueryModels;
using ContentService.Infrastructure.Persistence.Sql.Queries.Aggregates.Posts.Configurations;
using ContentService.Infrastructure.Persistence.Sql.Queries.QueryModels;
#endif
using MDF.Framework.Infrastructure.Conversions.Extensions;
using MDF.Framework.Infrastructure.Queries;
using MDF.Framework.SeedWork.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace ContentService.Infrastructure.Persistence.Sql.Queries.Common;

public partial class ContentServiceQueryDbContext : BaseQueryDbContext
{
	public ContentServiceQueryDbContext(DbContextOptions<ContentServiceQueryDbContext> options)
		: base(options)
	{
	}

	//باید به مدل های همین پروژه اشاره کنیم
#if (IncludeExample)
	public virtual DbSet<CommentQuery> Comments { get; set; }
	public virtual DbSet<PostQuery> Posts { get; set; }
	public virtual DbSet<CategoryQuery> Categories { get; set; }
#endif
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
		base.OnConfiguring(optionsBuilder);
	}
	protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
	{
		base.ConfigureConventions(configurationBuilder);
		configurationBuilder.UseDateTimeAsUtcConversion();
		configurationBuilder.UseNullableDateTimeAsUtcConversion();
	}
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
#if (IncludeExample)
		modelBuilder.Entity<CommentQuery>(entity =>
		{
			entity.ToTable("Comments");
			entity.HasKey(e => new { e.PostId, e.Id });

			entity.ToTable("Comments");

			entity.Property(e => e.CommentText)
				.IsRequired()
				.HasMaxLength(255);
			entity.Property(e => e.Email).IsRequired();
			entity.Property(e => e.Name)
				.IsRequired()
				.HasMaxLength(15);

			entity.HasOne(d => d.Post).WithMany(p => p.Comments).HasForeignKey(d => d.PostId);
		});

		modelBuilder.Entity<PostQuery>(entity =>
		{
			entity.ToTable("Posts");
			entity.Property(e => e.Id).ValueGeneratedNever();
			entity.Property(e => e.CategoryIds)
				.HasConversion(
					v => string.Join(',', v),
					v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(c =>(Guid?) GuidId.Create(c).Value ).ToList());
			entity.Property(e => e.Description)
				.IsRequired()
				.HasMaxLength(160);
			entity.Property(e => e.Text).IsRequired();
			entity.Property(e => e.Title)
				.IsRequired()
				.HasMaxLength(60);
		});

		modelBuilder.Entity<CategoryQuery>(entity =>
		{
			entity.ToTable("Categories");
			entity.Property(e => e.Id).ValueGeneratedNever();
			entity.Property(e => e.CategoryTitle)
				.IsRequired()
				.HasMaxLength(60);
			entity.Property(e => e.ParentCategoriesId)
				.HasConversion(
					v => string.Join(',', v),
					v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList<Guid>());
			entity.Property(e => e.PostIds)
				.HasConversion(
					v => string.Join(',', v),
					v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList());
		});
		//modelBuilder.ApplyConfigurationsFromAssembly
		// (typeof(PostQueryConfiguration).Assembly);
		OnModelCreatingPartial(modelBuilder);
#endif
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
