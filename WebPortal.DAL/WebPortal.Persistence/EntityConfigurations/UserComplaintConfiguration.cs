using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebPortal.Domain.Complaint;

namespace WebPortal.Persistence.EntityConfigurations;

public class UserComplaintConfiguration : IEntityTypeConfiguration<UserComplaint>
{
    public void Configure(EntityTypeBuilder<UserComplaint> builder)
    {
        builder.ToTable("complaints_user");
        builder.HasKey(complaint => complaint.Id);

        builder.HasOne(complaint => complaint.User)
            .WithMany(user => user.Complaints);
    }
}