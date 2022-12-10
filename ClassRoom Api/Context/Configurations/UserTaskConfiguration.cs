using ClassRoom_Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassRoom_Api.Context.Configurations;
public class UserTaskConfiguration : IEntityTypeConfiguration<UserTask>
{
    public void Configure(EntityTypeBuilder<UserTask> builder)
    {
        builder.ToTable("user_tasks");
        builder.HasKey(t => t.Id);
        builder.Property(task => task.Description)
            .IsRequired()
            .HasColumnName("description")
            .HasMaxLength(50)
            .HasDefaultValue("user task description");
    }
}
