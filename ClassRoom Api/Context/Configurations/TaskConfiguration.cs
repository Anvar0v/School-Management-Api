using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassRoom_Api.Context.Configurations;
public class TaskConfiguration
{
    public static void Configure(
        EntityTypeBuilder<ClassRoom_Api.Entities.Task> builder)
    {
        builder.ToTable("tasks");
        builder.HasKey(x => x.Id);
        builder.Property(task => task.Title)
            .IsRequired()
            .HasColumnName("title")
            .HasMaxLength(50)
            .HasDefaultValue("task title");

        builder.Property(task => task.Description)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(task => task.MaxScore)
            .IsRequired()
            .HasMaxLength(50);
    }
}
