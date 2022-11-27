using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebPortal.Domain;

namespace WebPortal.Persistence.EntityConfigurations;

public class CommentaryConfiguration : IEntityTypeConfiguration<Commentary>
{
    public void Configure(EntityTypeBuilder<Commentary> builder)
    {
        builder.ToTable("commentaries");
        builder.HasKey(commentary => commentary.Id);

        builder.HasOne(commentary => commentary.Article)
            .WithMany(article => article.Commentaries)
            .HasForeignKey(commentary => commentary.ArticleId);
        
        builder.HasOne(commentary => commentary.Author)
            .WithMany(user => user.Commentaries)
            .HasForeignKey(commentary => commentary.AuthorId);

        builder.HasMany(commentary => commentary.Replies);
        builder.HasOne(commentary => commentary.Parent);
    }
}