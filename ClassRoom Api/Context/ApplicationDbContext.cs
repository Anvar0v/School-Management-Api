using ClassRoom_Api.Context.Configurations;
using ClassRoom_Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task = ClassRoom_Api.Entities.Task;

namespace ClassRoom_Api.Context;
public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<UserCourse> UserCourses { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<UserTask> UserTasks { get; set; }
    public DbSet<TaskComment> TaskComments { get; set; }
    public DbSet<LocalizedStringEntity> LocalizedStrings { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<LocalizedStringEntity>()
            .HasKey(local => local.Key);

        builder.Entity<LocalizedStringEntity>()
            .Property(local => local.Uz).IsRequired().HasDefaultValue("uzbekcha");

        TaskConfiguration.Configure(builder.Entity<Task>());

        new UserTaskConfiguration().Configure(builder.Entity<UserTask>());  

        builder.ApplyConfigurationsFromAssembly
            (typeof(ApplicationDbContext).Assembly);

        builder.Entity<LocalizedStringEntity>().HasData(
            new List<LocalizedStringEntity>()
            {
                new LocalizedStringEntity()
                {
                    Key = "Required",
                    Uz = "{0} kiritilishi kerak",
                    Ru = "{0} поле должно быть заполнено",
                    En = "{0} field is required"
                }
            });
    }

}
