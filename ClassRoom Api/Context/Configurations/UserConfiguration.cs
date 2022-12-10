using ClassRoom_Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassRoom_Api.Context.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(x => x.Id);
        builder.Property(user => user.Firstname).IsRequired();

        builder.HasMany(user => user.Courses)
            .WithOne(userCourse => userCourse.User)
            .HasForeignKey(userCourse => userCourse.UserId);
    }
}
