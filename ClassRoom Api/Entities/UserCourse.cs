using System.ComponentModel.DataAnnotations.Schema;

namespace ClassRoom_Api.Entities;
public class UserCourse
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual User? User { get; set; }

    public Guid CourseId { get; set; }
    [ForeignKey("CourseId")]
    public virtual Course? Course { get; set; }
    public bool isAdmin { get; set; }
}
