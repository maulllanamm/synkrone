using System.ComponentModel.DataAnnotations;

namespace synkrone.Model.Entities;

public class User
{
    public Guid Id { get; set; }
        
    [Required]
    [StringLength(100)]
    public string Username { get; set; }
        
    [Required]
    [StringLength(100)]
    public string Email { get; set; }
        
    [Required]
    public string PasswordHash { get; set; }
        
    [StringLength(200)]
    public string? DisplayName { get; set; }
        
    public string? ProfilePicture { get; set; }
        
    public string? Bio { get; set; }
        
    public bool IsOnline { get; set; }
        
    public DateTime LastSeen { get; set; }
        
    public DateTime CreatedAt { get; set; }
        
    public DateTime UpdatedAt { get; set; }
}