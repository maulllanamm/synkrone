using System.ComponentModel.DataAnnotations;

namespace synkrone.Model.Entities;

public class Group
{
    public Guid Id { get; set; }
        
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
        
    public string? Description { get; set; }
        
    public string? GroupPicture { get; set; }
        
    public GroupType Type { get; set; } = GroupType.Public;
        
    public DateTime CreatedAt { get; set; }
        
    public DateTime UpdatedAt { get; set; }
        
    public Guid CreatedById { get; set; }
    public User CreatedBy { get; set; }
        
    // Navigation Properties
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}
    
public enum GroupType
{
    Public,
    Private
}