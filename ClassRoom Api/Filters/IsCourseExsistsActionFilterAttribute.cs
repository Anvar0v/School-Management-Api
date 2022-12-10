using ClassRoom_Api.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace ClassRoom_Api.Filters;
public class IsCourseExsistsActionFilterAttribute : ActionFilterAttribute
{
    private readonly ApplicationDbContext _context;

    public IsCourseExsistsActionFilterAttribute(ApplicationDbContext context)
    {
        _context = context;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ActionArguments.ContainsKey("courseId"))
        {
            await next();
            return;
        }

        //getting the value of the course through passed argument;
        var courseId = (Guid?)context.ActionArguments["courseId"];

        //if there is no course with id that matches, then we haev to return NotFoundResult;
        if(!await _context.Courses.AnyAsync(course => course.Id == courseId))
        {
            context.Result = new NotFoundResult();
            return;
        }

        await next();
    }

}
