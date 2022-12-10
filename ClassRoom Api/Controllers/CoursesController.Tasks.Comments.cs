using ClassRoom_Api.Entities;
using ClassRoom_Api.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassRoom_Api.Controllers;
public partial class CoursesController : ControllerBase
{
    [HttpGet("{courseId}/tasks{taskId}/comments")]
    [ProducesResponseType(typeof(List<TaskCommentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTaskComments(Guid courseId, Guid taskId)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.CourseId == courseId);
        if (task is null)
            return NotFound();

        //for storing all comments and returning to the user;
        var comments = new List<TaskCommentDto>();
        if (task.TaskComments is null)
            return Ok(comments);

        var mainComments = task.TaskComments.Where(tc => tc.ParentId == null).ToList();

        foreach (var comment in mainComments)
        {
            var commentDto = ToTaskCommentDto(comment);
            comments.Add(commentDto);
        }

        return Ok(comments);
    }

    [HttpPost("{courseId}/tasks/{taskId}/comments")]
    public async Task<IActionResult> AddTaskComment(Guid courseId, Guid taskId,
        [FromBody] CreateTaskCommentDto taskCommentDto)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.CourseId == courseId);
        if (task is null)
            return NotFound();

        var user = await _userManager.GetUserAsync(User);

        task.TaskComments ??= new List<TaskComment>();

        task.TaskComments.Add(new TaskComment()
        {
            TaskId = taskId,
            UserId = user.Id,
            Comment = taskCommentDto.Comment,
            ParentId = taskCommentDto.ParentId, //id of the main comment
        });

        await _context.SaveChangesAsync();
        return Ok();
    }

    private TaskCommentDto ToTaskCommentDto(TaskComment comment)
    {
        var commentDto = new TaskCommentDto()
        {
            Id = comment.Id,
            Comment = comment.Comment,
            CreatedDate = comment.CreatedDate,
            User = comment.User?.Adapt<UserDto>(),
        };

        //if there are no reply(children) comments we have to return our dto object
        if (comment.Children is null)
            return commentDto;

        foreach (var child in comment.Children)
        {
            commentDto.Children ??= new List<TaskCommentDto>();
            //here i am adding a reply after enumerating it to my list as a child of a comment
            commentDto.Children.Add(ToTaskCommentDto(child));
        }
        return commentDto;
    }
}
