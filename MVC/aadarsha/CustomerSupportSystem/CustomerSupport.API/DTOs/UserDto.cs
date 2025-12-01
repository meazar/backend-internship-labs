namespace CustomerSupport.API.DTOs;

public record UserDto(int Id, string? Name, string? Email, string? user_type, DateTime CreatedAt);
