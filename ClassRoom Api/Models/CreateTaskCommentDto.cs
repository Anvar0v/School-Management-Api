using System.ComponentModel.DataAnnotations;

namespace ClassRoom_Api.Models; 
public class CreateTaskCommentDto
{
    [Required]
    public string?  Comment { get; set; }
    public Guid? ParentId { get; set; }
}
