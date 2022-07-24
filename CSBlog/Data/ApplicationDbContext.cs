using CSBlog.Models.Blog;
using CSBlog.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CSBlog.Data;

public sealed class ApplicationDbContext : IdentityDbContext<BlogUser>
{
  public DbSet<BlogUser> BlogUsers { get; set; }

  public DbSet<Article> Articles { get; set; }

  public DbSet<Comment> Comments { get; set; }

  public DbSet<Tag> Tags { get; set; }


  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
  {
    // Database.EnsureDeleted();
    Database.EnsureCreated();
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // modelBuilder.Entity<ArticleTag>()
    //   .HasOne(at => at.Article)
    //   .WithMany(a => a.ArticleTags)
    //   .HasForeignKey(ai => ai.ArticleId);
    //
    // modelBuilder.Entity<ArticleTag>()
    //   .HasOne(at => at.Tag)
    //   .WithMany(t => t.ArticleTags)
    //   .HasForeignKey(ti => ti.TagId);

    modelBuilder.Entity<Article>()
      .HasMany(b => b.Comments)
      .WithOne();

    modelBuilder
      .Entity<Article>()
      .HasMany(p => p.Tags)
      .WithMany(p => p.Articles)
      .UsingEntity(j => j.ToTable("ArticleTags"));
    modelBuilder.ApplyConfiguration(new BlogUserEntityConfiguration());
  }
}

public class BlogUserEntityConfiguration : IEntityTypeConfiguration<BlogUser>
{
  public void Configure(EntityTypeBuilder<BlogUser> builder)
  {
    builder.Property(u => u.FirstName).HasMaxLength(32);
    builder.Property(u => u.LastName).HasMaxLength(32);
  }
}