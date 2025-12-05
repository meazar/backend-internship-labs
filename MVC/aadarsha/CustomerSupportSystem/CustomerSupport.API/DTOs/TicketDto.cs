namespace CustomerSupport.API.DTOs;

public record TicketDto(
    int Id,
    string? TicketNumber,
    string? Subject,
    string? Description,
    int CustomerId,
    int? AssignedAgentId,
    int? TeamId,
    string? Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
