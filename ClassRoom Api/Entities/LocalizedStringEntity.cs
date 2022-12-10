using System.ComponentModel.DataAnnotations;

namespace ClassRoom_Api.Entities;
public class LocalizedStringEntity
{
    [Key]
    public string? Key { get; set; }
    public string? Uz { get; set; }
    public string? En { get; set; }
    public string? Ru { get; set; }
}
