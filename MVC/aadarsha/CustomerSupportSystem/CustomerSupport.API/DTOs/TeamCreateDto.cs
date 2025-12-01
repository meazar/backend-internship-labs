namespace CustomerSupport.API.DTOs;

public class TeamCreateDto
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}
