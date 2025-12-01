namespace CustomerSupport.API.DTOs.Auth;

public class AuthResponseDto
{
    public string AccessToken { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public int UserId { get; set; }
    public string UserType { get; set; } = null!;
    public string Email { get; set; } = null!;
}
