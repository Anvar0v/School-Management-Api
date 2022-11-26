using ClassRoom_Api.Entities;

namespace ClassRoom_Api.Models;
public class UserTaskResultDto : TaskDto 
{
    //todo here taskDto is only for tasks, props like title, description is for task itself;
    public UserTaskResult? UserResult { get; set; }
}

public class UserTaskResult
{
    public string? Description { get; set; }
    public EUserTaskStatus Status { get; set; }
}

public class UsersTaskResult : UserTaskResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}