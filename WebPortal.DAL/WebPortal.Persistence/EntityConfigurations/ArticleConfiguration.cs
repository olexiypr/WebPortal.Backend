using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebPortal.Domain;
using WebPortal.Domain.Enums;
using WebPortal.Domain.User;

namespace WebPortal.Persistence.EntityConfigurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("articles");
        builder.HasKey(article => article.Id);
        builder.Property(article => article.Text)
            .HasColumnType("text");

        builder.HasOne(article => article.Category)
            .WithMany(category => category.Articles)
            .HasForeignKey(article => article.CategoryId);
        
        builder.HasOne(article => article.Author)
            .WithMany(user => user.Articles)
            .HasForeignKey(article => article.AuthorId);

        builder.HasMany(article => article.Tags)
            .WithMany(tag => tag.Articles);

        builder.HasMany(article => article.Commentaries)
            .WithOne(commentary => commentary.Article);
        
        builder.Property(article => article.KeyWords)
            .HasConversion(v => string.Join(" ", v.ToArray()),
                 v => v.Split(' ', StringSplitOptions.None).ToList());
        
    }
}