using CustomerSupport.API.Models;

namespace CustomerSupport.API.Services;

public interface IAutoAssignmentService
{
    Task<int?> FindBestAgentAsync(int? teamId = null);
    Task<bool> AutoAssignTicketAsync(int ticketId);
}
