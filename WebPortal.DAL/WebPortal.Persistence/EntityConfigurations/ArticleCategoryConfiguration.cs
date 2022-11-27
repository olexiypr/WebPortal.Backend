using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebPortal.Domain;

namespace WebPortal.Persistence.EntityConfigurations;

public class ArticleCategoryConfiguration : IEntityTypeConfiguration<ArticleCategory>
{
    public void Configure(EntityTypeBuilder<ArticleCategory> builder)
    {
        builder.ToTable("article_categories");
        builder.HasKey(category => category.Id);

        builder.HasMany(category => category.Articles)
            .WithOne(article => article.Category);
    }
}