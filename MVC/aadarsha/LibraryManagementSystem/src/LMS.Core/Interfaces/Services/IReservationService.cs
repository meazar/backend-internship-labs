using LMS.Core.DTOs.Requests;
using LMS.Core.DTOs.Responses;

namespace LMS.Core.Interfaces.Services;

public interface IReservationService
{
    Task<ApiResponse<ReservationResponse>> CreateReservationAsync(CreateReservationRequest request);
    Task<ApiResponse<ReservationResponse>> GetReservationByIdAsync(int id);
    Task<ApiResponse<IEnumerable<ReservationResponse>>> GetReservationsByMemberAsync(int memberId);
    Task<ApiResponse<IEnumerable<ReservationResponse>>> GetReservationsByBookAsync(int bookId);
    Task<ApiResponse<ReservationResponse>> CancelReservationAsync(int reservationId);
    Task<ApiResponse<ReservationResponse>> FulfillReservationAsync(int reservationId);
    Task<ApiResponse<bool>> NotifyNextInQueueAsync(int bookId);
    Task<ApiResponse<int>> GetQueuePositionAsync(int reservationId);
    Task<ApiResponse<bool>> CheckReservationEligibilityAsync(int memberId, int bookId);
}
