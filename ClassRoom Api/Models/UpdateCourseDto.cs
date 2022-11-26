using System.ComponentModel.DataAnnotations;

namespace ClassRoom_Api.Models;
public class UpdateCourseDto
{
    [Required]
    public string? Name { get; set; }
}
