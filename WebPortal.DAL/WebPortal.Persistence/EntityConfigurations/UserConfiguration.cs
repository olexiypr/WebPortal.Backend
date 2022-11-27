using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebPortal.Domain;
using WebPortal.Domain.User;

namespace WebPortal.Persistence.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasMany(user => user.Articles)
            .WithOne(article => article.Author);

        builder.HasMany(user => user.Complaints)
            .WithOne(complaint => complaint.User);

        builder.HasOne(user => user.Recommendation)
            .WithOne(recommendation => recommendation.User)
            .HasForeignKey<Recommendation>(recommendation => recommendation.UserId);
        
    }
}