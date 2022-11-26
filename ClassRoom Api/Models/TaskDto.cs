using ClassRoom_Api.Entities;

namespace ClassRoom_Api.Models;
public class TaskDto
{
    public Guid Id { get; set; }
    public string?  Title { get; set; }
    public string?  Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    //for setting status to task, itially it is gonna be created, then in process(todo status)
    public ETaskStatus Status { get; set; }
}
