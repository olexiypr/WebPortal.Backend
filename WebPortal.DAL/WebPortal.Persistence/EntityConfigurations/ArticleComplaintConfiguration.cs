using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebPortal.Domain.Complaint;

namespace WebPortal.Persistence.EntityConfigurations;

public class ArticleComplaintConfiguration : IEntityTypeConfiguration<ArticleComplaint>
{
    public void Configure(EntityTypeBuilder<ArticleComplaint> builder)
    {
        builder.ToTable("complaints_article");
        builder.HasKey(complaint => complaint.Id);

        builder.HasOne(complaint => complaint.Article)
            .WithMany(article => article.Complaints);
    }
}