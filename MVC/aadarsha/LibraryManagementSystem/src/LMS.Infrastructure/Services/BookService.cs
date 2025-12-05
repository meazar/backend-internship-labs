using LMS.Core.DTOs.Requests;
using LMS.Core.DTOs.Responses;
using LMS.Core.Entities;
using LMS.Core.Interfaces.Repositories;
using LMS.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace LMS.Infrastructure.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BookService> _logger;

    public BookService(
        IBookRepository bookRepository,
        IUnitOfWork unitOfWork,
        ILogger<BookService> logger
    )
    {
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ApiResponse<BookResponse>> CreateBookAsync(CreateBookRequest req)
    {
        try
        {
            if (
                !string.IsNullOrWhiteSpace(req.ISBN)
                && (await _bookRepository.GetByIsbnAsync(req.ISBN)) != null
            )
            {
                return ApiResponse<BookResponse>.ErrorResponse(
                    "A book with the same ISBN already exists."
                );
            }

            var book = new Book
            {
                Title = req.Title,
                ISBN = req.ISBN,
                TotalCopies = req.TotalCopies,
                AvailableCopies = req.TotalCopies,
                Description = req.Description ?? string.Empty,
                PublicationYear = req.PublicationYear,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            await _bookRepository.AddAsync(book);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Book created with ID: {BookId}", book.Id);

            var response = MapToResponse(book);

            return ApiResponse<BookResponse>.SuccessResponse(
                response,
                "Book created successfully."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating book: {ErrorMessage}", ex.Message);
            return ApiResponse<BookResponse>.ErrorResponse(
                "An error occurred while creating the book."
            );
        }
    }

    public async Task<ApiResponse<BookResponse>> GetBookByIdAsync(int bookId)
    {
        try
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
            {
                return ApiResponse<BookResponse>.ErrorResponse("Book not found.");
            }

            var response = MapToResponse(book);

            return ApiResponse<BookResponse>.SuccessResponse(
                response,
                "Book retrieved successfully."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error retrieving book {BookId}:{ErrorMessage}",
                bookId,
                ex.Message
            );
            return ApiResponse<BookResponse>.ErrorResponse(
                "An error occurred while retrieving the book."
            );
        }
    }

    public async Task<ApiResponse<BookResponse>> GetBookByTitleAsync(string title)
    {
        try
        {
            var book = await _bookRepository.GetByTitleAsync(title);
            if (book == null)
            {
                return ApiResponse<BookResponse>.ErrorResponse("Book not found.");
            }

            var response = MapToResponse(book);

            return ApiResponse<BookResponse>.SuccessResponse(
                response,
                "Book retrieved successfully."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error retrieving book with title {Title}:{ErrorMessage}",
                title,
                ex.Message
            );
            return ApiResponse<BookResponse>.ErrorResponse(
                "An error occurred while retrieving the book."
            );
        }
    }

    public async Task<ApiResponse<IEnumerable<BookResponse>>> GetBooksByAuthorAsync(
        string authorName
    )
    {
        try
        {
            var books = await _bookRepository.GetByAuthorNameAsync(authorName);
            if (books == null || !books.Any())
            {
                return ApiResponse<IEnumerable<BookResponse>>.ErrorResponse(
                    "No books found for this author."
                );
            }

            var responses = books.Select(MapToResponse).ToList();
            return ApiResponse<IEnumerable<BookResponse>>.SuccessResponse(
                responses,
                $"Found {books.Count()} book(s) by author."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching books by author: {Author}", authorName);
            return ApiResponse<IEnumerable<BookResponse>>.ErrorResponse(
                "An error occurred while fetching books by author."
            );
        }
    }

    public async Task<ApiResponse<BookResponse>> GetBookByIsbnAsync(string isbn)
    {
        try
        {
            var book = await _bookRepository.GetByIsbnAsync(isbn);
            if (book == null)
            {
                return ApiResponse<BookResponse>.ErrorResponse("Book not found.");
            }

            var response = MapToResponse(book);

            return ApiResponse<BookResponse>.SuccessResponse(
                response,
                "Book retrieved successfully."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error retrieving book with ISBN {ISBN}:{ErrorMessage}",
                isbn,
                ex.Message
            );
            return ApiResponse<BookResponse>.ErrorResponse(
                "An error occurred while retrieving the book."
            );
        }
    }

    public async Task<ApiResponse<IEnumerable<BookResponse>>> GetAllBooksAsync(
        int pageNumber = 1,
        int pageSize = 10
    )
    {
        try
        {
            var books = await _bookRepository.GetPagedAsync(pageNumber, pageSize);
            var responses = books.Select(MapToResponse);

            return ApiResponse<IEnumerable<BookResponse>>.SuccessResponse(
                responses,
                "All available books retrieved successfully."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all books: {ErrorMessage}", ex.Message);
            return ApiResponse<IEnumerable<BookResponse>>.ErrorResponse(
                "An error occurred while retrieving books."
            );
        }
    }

    public async Task<ApiResponse<IEnumerable<BookResponse>>> SearchBooksAsync(string term)
    {
        try
        {
            var books = await _bookRepository.SearchBooksAsync(term);
            var responses = books.Select(MapToResponse);

            return ApiResponse<IEnumerable<BookResponse>>.SuccessResponse(
                responses,
                "Books retrieved successfully."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error searching books with term {term}: {ErrorMessage}",
                term,
                ex.Message
            );
            return ApiResponse<IEnumerable<BookResponse>>.ErrorResponse(
                "An error occurred while searching books."
            );
        }
    }

    public async Task<ApiResponse<BookResponse>> UpdateBookAsync(int id, CreateBookRequest req)
    {
        try
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return ApiResponse<BookResponse>.ErrorResponse("Book not found.");
            }

            if (!string.IsNullOrWhiteSpace(req.ISBN) && req.ISBN != book.ISBN)
            {
                var existingBookWithIsbn = await _bookRepository.GetByIsbnAsync(req.ISBN);
                if (existingBookWithIsbn != null)
                {
                    return ApiResponse<BookResponse>.ErrorResponse(
                        "A book with the same ISBN already exists."
                    );
                }
            }

            int newAvailableCopies = book.AvailableCopies;
            if (req.TotalCopies != book.TotalCopies)
            {
                int difference = req.TotalCopies - book.TotalCopies;
                newAvailableCopies += difference;

                if (newAvailableCopies < 0)
                {
                    return ApiResponse<BookResponse>.ErrorResponse(
                        "Borrowed copies cannot be greater than total copies."
                    );
                }
            }

            book.Title = req.Title;
            book.ISBN = req.ISBN;
            book.TotalCopies = req.TotalCopies;
            book.AvailableCopies = newAvailableCopies;
            book.Description = req.Description ?? string.Empty;
            book.PublicationYear = req.PublicationYear;
            book.UpdatedAt = DateTime.UtcNow;

            await _bookRepository.UpdateAsync(book);
            await _unitOfWork.CompleteAsync();

            var response = MapToResponse(book);
            return ApiResponse<BookResponse>.SuccessResponse(
                response,
                "Book updated successfully."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating book {BookId}: {ErrorMessage}", id, ex.Message);
            return ApiResponse<BookResponse>.ErrorResponse(
                "An error occurred while updating the book."
            );
        }
    }

    public async Task<ApiResponse<bool>> DeleteBookAsync(int id)
    {
        try
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return ApiResponse<bool>.ErrorResponse("Book not found.");
            }

            // soft delete
            book.IsActive = false;
            book.UpdatedAt = DateTime.UtcNow;

            await _bookRepository.UpdateAsync(book);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Book deactivated with ID: {BookId}", id);

            return ApiResponse<bool>.SuccessResponse(true, "Book deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting book {BookId}: {ErrorMessage}", id, ex.Message);
            return ApiResponse<bool>.ErrorResponse("An error occurred while deleting the book.");
        }
    }

    public async Task<ApiResponse<int>> GetAvailableCopiesAsync(int bookId)
    {
        try
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
            {
                return ApiResponse<int>.ErrorResponse("Book not found.");
            }
            return ApiResponse<int>.SuccessResponse(
                book.AvailableCopies,
                "Available copies retrieved successfully."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error getting available copies for book {BookId}: {ErrorMessage}",
                bookId,
                ex.Message
            );
            return ApiResponse<int>.ErrorResponse(
                "An error occurred while getting available copies."
            );
        }
    }

    // Helper methods: ErrorResponse, SuccessResponse, MapToResponse
    private BookResponse MapToResponse(Book book)
    {
        return new BookResponse
        {
            Id = book.Id.GetHashCode(),
            Title = book.Title,
            ISBN = book.ISBN,
            TotalCopies = book.TotalCopies,
            AvailableCopies = book.AvailableCopies,
            Description = book.Description,
            PublicationYear = book.PublicationYear,
            IsActive = book.IsActive,
            CreatedAt = book.CreatedAt,
        };
    }

    private ApiResponse<T> ErrorResponse<T>(string message)
    {
        return ApiResponse<T>.ErrorResponse(message);
    }

    private ApiResponse<T> SuccessResponse<T>(T data, string message = "")
    {
        return ApiResponse<T>.SuccessResponse(data, message);
    }
}
