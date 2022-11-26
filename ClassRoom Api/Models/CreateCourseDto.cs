using System.ComponentModel.DataAnnotations;

namespace ClassRoom_Api.Models;
public class CreateCourseDto
{
    [Required]
    public string? Name { get; set; }
}
