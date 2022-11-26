using ClassRoom_Api.Entities;

namespace ClassRoom_Api.Models;
public class CreateUserTaskResultDto
{
    public string? Description { get; set; }
    public EUserTaskStatus Status { get; set; }
}
