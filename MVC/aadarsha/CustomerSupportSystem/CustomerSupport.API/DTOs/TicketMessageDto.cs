namespace CustomerSupport.API.DTOs;

public record TicketMessageDto(
    int Id,
    int TicketId,
    int AuthorId,
    string? Content,
    DateTime CreatedAt
);
