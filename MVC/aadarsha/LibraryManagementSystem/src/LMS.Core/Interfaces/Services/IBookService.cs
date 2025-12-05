using LMS.Core.DTOs.Requests;
using LMS.Core.DTOs.Responses;

namespace LMS.Core.Interfaces.Services;

public interface IBookService
{
    Task<ApiResponse<BookResponse>> CreateBookAsync(CreateBookRequest request);
    Task<ApiResponse<BookResponse>> GetBookByIdAsync(int id);
    Task<ApiResponse<BookResponse>> GetBookByIsbnAsync(string isbn);
    Task<ApiResponse<IEnumerable<BookResponse>>> GetAllBooksAsync(
        int pageNumber = 1,
        int pageSize = 10
    );
    Task<ApiResponse<IEnumerable<BookResponse>>> SearchBooksAsync(string searchTerm);
    Task<ApiResponse<BookResponse>> UpdateBookAsync(int id, CreateBookRequest request);
    Task<ApiResponse<bool>> DeleteBookAsync(int id);
    Task<ApiResponse<int>> GetAvailableCopiesAsync(int id);
}
