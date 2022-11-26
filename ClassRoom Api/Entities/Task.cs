using System.ComponentModel.DataAnnotations.Schema;

namespace ClassRoom_Api.Entities;
public class Task
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public ETaskStatus Status { get; set; }
    public int MaxScore { get; set; }

    public Guid CourseId { get; set; }
    [ForeignKey(nameof(CourseId))]
    public virtual Course? Course { get; set; }

    public virtual List<UserTask>? UserTasks { get; set; }

    public virtual List<TaskComment>? TaskComments { get; set; }
}

