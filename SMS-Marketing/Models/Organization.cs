using System.ComponentModel.DataAnnotations;

namespace SMS_Marketing.Models;

public class Organization
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string ManagerId { get; set; } = string.Empty;
    [Required]
    public string ManagerName { get; set; } = string.Empty;
    public string Users { get; set; } = string.Empty;
    public bool IsActive { get; set; } = false;
    public bool IsTwitter { get; set; } = false;
    public bool IsSMS { get; set; } = false;
    public bool IsFacebook { get; set; } = false;
}
