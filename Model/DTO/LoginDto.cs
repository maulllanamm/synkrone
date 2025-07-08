using System.ComponentModel.DataAnnotations;

namespace synkrone.Model.DTO;

public class LoginDto
{
    [Required]
    public string UsernameOrEmail { get; set; }
        
    [Required]
    public string Password { get; set; }
}