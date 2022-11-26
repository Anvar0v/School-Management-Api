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

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

}
