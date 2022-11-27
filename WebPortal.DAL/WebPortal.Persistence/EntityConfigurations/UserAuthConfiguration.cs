using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebPortal.Domain.User;

namespace WebPortal.Persistence.EntityConfigurations;

public class UserAuthConfiguration : IEntityTypeConfiguration<UserAuth>
{
    public void Configure(EntityTypeBuilder<UserAuth> builder)
    {
        builder.ToTable("user_auth");
        builder.HasKey(auth => auth.Id);
        builder.HasAlternateKey(auth => auth.Email);
        
    }
}