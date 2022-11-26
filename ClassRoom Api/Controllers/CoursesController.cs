using ClassRoom_Api.Context;
using ClassRoom_Api.Entities;
using ClassRoom_Api.Mappers;
using ClassRoom_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassRoom_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public partial class CoursesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    public CoursesController(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetCourses()
    {
        var courses = await _context.Courses.ToListAsync();

        //gets particular course and all user that attend this course
        List<CourseDto> coursesDto = courses.Select(c => c.ToDto()).ToList();

        return Ok(coursesDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCoursebyId(Guid id)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);

        if (course is null)
            return NotFound();

        return Ok(course.ToDto());
    }

    [HttpPost]
    public async Task<IActionResult> CrteateCourse([FromBody] CreateCourseDto createCourseDto)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var user = await _userManager.GetUserAsync(User);

        var course = new Course()
        {
            Name = createCourseDto.Name,
            Key = Guid.NewGuid().ToString("N"),
            //adds to list user that is related to a particular course =>
            //if user crates a new room then he is gonna be an admin
            Users = new List<UserCourse>()
            {
                new UserCourse()
                {
                    UserId = user.Id,
                    isAdmin = true,
                }
            }
        };

        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();

        course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == course.Id);

        return Ok(course?.ToDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] UpdateCourseDto updateCourseDto)
    {
        if (!await _context.Courses.AnyAsync(c => c.Id == id))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);

        if (course is null)
            return NotFound();

        var user = await _userManager.GetUserAsync(User);

        //if there is no such user that id does not match and if user is not admin it then returns BadRequest
        if (course.Users?.Any(uc => uc.UserId == user.Id && uc.isAdmin) != true)
            return BadRequest();

        course.Name = updateCourseDto.Name;
        await _context.SaveChangesAsync();

        return Ok(course?.ToDto());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(Guid id)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);

        //if there is no such course that mathes to parameter that passed it then returns NOT FOND
        if (course is null)
            return NotFound();

        var user = await _userManager.GetUserAsync(User);
        if (course.Users?.Any(uc => uc.Id == user.Id && uc.isAdmin) != true)
            return BadRequest(); //or Forbid

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("{id}/join")]
    public async Task<IActionResult> JoinCourse(Guid id,
        [FromBody] JoinCourseDto joinCourseDto)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
        if (course is null)
            return NotFound();

        var user = await _userManager.GetUserAsync(User);
        // if user with such id exists it'll return badrequest because he is already joined to course;
        if (course.Users?.Any(uc => uc.UserId == user.Id) == true)
            return BadRequest();

        _context.UserCourses.Add(new UserCourse()
        {
            UserId = user.Id,
            CourseId = course.Id,
            isAdmin = false,
        });

        await _context.SaveChangesAsync();
        return Ok();
    }

}
