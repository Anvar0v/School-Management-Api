using ClassRoom_Api.Context;
using ClassRoom_Api.Entities;
using ClassRoom_Api.Mappers;
using ClassRoom_Api.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassRoom_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _context;

    public ProfileController(UserManager<User> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    [HttpGet("courses")]
    [ProducesResponseType(typeof(List<CourseDto>),StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCourses()
    {
        var user = await _userManager.GetUserAsync(User);
        //todo courses that are related to a particular user
        var courseDto = user.Courses?.Select(uc => uc.Course?.ToDto()).ToList();

        return Ok(courseDto);
    }

    [HttpGet("courses/{courseId}/tasks")]
    [ProducesResponseType(typeof(List<UserTaskResultDto>),StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserTasks(Guid courseId)
    {
        var user = await _userManager.GetUserAsync(User);

        var course = await _context.Courses.FirstOrDefaultAsync();

        if (course?.Tasks is null)
            return BadRequest();

        var tasks = course.Tasks;

        var userTasks = new List<UserTaskResultDto>();

        foreach (var task in tasks)
        {
            var result = task.Adapt<UserTaskResultDto>();
            var userResultEntity = task.UserTasks?.FirstOrDefault(ut => ut.UserId == user.Id);

            result.UserResult = userResultEntity == null ? null : new UserTaskResult()
            {
                Status = userResultEntity.Status,
                Description = userResultEntity.Description,
            };

            userTasks.Add(result);
        }
        return Ok(userTasks);
    }


    [HttpPost("course/{courseId}/tasks/{taskId}")]
    //todo this action for showing whether student completed , doing task
    public async Task<IActionResult> AddUserTaskResult(Guid courseId, Guid taskId,
        [FromBody] CreateUserTaskResultDto resultDto)
    {
        //whether such course and task exist with ginven id
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.CourseId == courseId);
        if (task is null)
            return NotFound();

        var user = await _userManager.GetUserAsync(User);

        var userTaskResult = await _context.UserTasks
            .FirstOrDefaultAsync(ut => ut.UserId == user.Id && ut.TaskId == taskId);

        //todo if there is no such user  in userTasks table
        // todo that is doing particular task then we have to add the user and task;
        if(userTaskResult is null)
        {
            userTaskResult = new UserTask()
            {
                UserId = user.Id,
                TaskId = taskId,
            };

            await _context.UserTasks.AddAsync(userTaskResult);
        }

        userTaskResult.Description = resultDto.Description;
        userTaskResult.Status = resultDto.Status;

        await _context.SaveChangesAsync();
        
        return Ok();
    }

}
