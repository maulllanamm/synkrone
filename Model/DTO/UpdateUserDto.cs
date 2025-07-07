using System.ComponentModel.DataAnnotations;

namespace synkrone.Model.DTO;

public class UpdateUserDto
{
    [StringLength(200)]
    public string? DisplayName { get; set; }
        
    public string? Bio { get; set; }
        
    public string? ProfilePicture { get; set; }
}