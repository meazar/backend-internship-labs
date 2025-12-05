using LMS.Core.Entities;

namespace LMS.Core.Interfaces.Repositories;

public interface IBookRepository : IGenericRepository<Book>
{
    Task<Book?> GetByTitleAsync(string title);
    Task<Book?> GetByIsbnAsync(string isbn);
    Task<IEnumerable<Book>> GetByAuthorNameAsync(string authorName);
    Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm);
    Task<Book?> UpdateBookAsync(int id, Book book);
    Task<bool> IsIsbnExistsAsync(string isbn);
}
