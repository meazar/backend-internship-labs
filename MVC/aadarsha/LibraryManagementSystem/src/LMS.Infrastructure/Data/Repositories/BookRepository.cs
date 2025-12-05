using LMS.Core.Entities;
using LMS.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Data.Repositories;

public class BookRepository : GenericRepository<Book>, IBookRepository
{
    public BookRepository(LMSDbContext context)
        : base(context) { }

    public async Task<Book?> GetByIsbnAsync(string isbn)
    {
        return await _dbSet
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .FirstOrDefaultAsync(b => b.ISBN == isbn);
    }

    public async Task<Book?> GetByTitleAsync(string title)
    {
        return await _dbSet
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .FirstOrDefaultAsync(b => b.Title == title);
    }

    public override async Task<Book?> GetByIdAsync(int bookId)
    {
        return await _dbSet
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .FirstOrDefaultAsync(b => b.Id == bookId);
    }

    // FIXED: Changed to search by author name through BookAuthors
    public async Task<IEnumerable<Book>> GetByAuthorNameAsync(string authorName)
    {
        return await _dbSet
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .Where(b =>
                b.BookAuthors.Any(ba =>
                    ba.Author.FirstName.Contains(authorName)
                    || ba.Author.LastName.Contains(authorName)
                )
            )
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
    {
        return await _dbSet
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .Where(b => b.AvailableCopies > 0 && b.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllAsync();

        var term = searchTerm.ToLower();
        return await _dbSet
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .Where(b =>
                b.Title.ToLower().Contains(term)
                || (b.ISBN != null && b.ISBN.ToLower().Contains(term))
                || b.BookAuthors.Any(ba =>
                    ba.Author.FirstName.ToLower().Contains(term)
                    || ba.Author.LastName.ToLower().Contains(term)
                )
            )
            .ToListAsync();
    }

    public async Task<Book?> UpdateBookAsync(int id, Book book)
    {
        var existingBook = await GetByIdAsync(id);
        if (existingBook == null)
        {
            return null;
        }

        // Update scalar properties only
        existingBook.Title = book.Title;
        // REMOVED: existingBook.Author = book.Author; // Author is not a scalar property
        existingBook.ISBN = book.ISBN;
        existingBook.TotalCopies = book.TotalCopies;
        existingBook.AvailableCopies = book.AvailableCopies;
        existingBook.PublicationYear = book.PublicationYear;
        existingBook.Description = book.Description;
        existingBook.Publisher = book.Publisher;
        existingBook.Language = book.Language;
        existingBook.NumberOfPages = book.NumberOfPages;
        existingBook.CoverImageUrl = book.CoverImageUrl;
        existingBook.UpdatedAt = DateTime.UtcNow;

        _dbSet.Update(existingBook);
        await _context.SaveChangesAsync();

        return existingBook;
    }

    public async Task<bool> IsIsbnExistsAsync(string isbn)
    {
        return await _dbSet.AnyAsync(b => b.ISBN == isbn);
    }

    public string GetAuthorNames(Book book)
    {
        if (book.BookAuthors == null || !book.BookAuthors.Any())
            return "Unknown Author";

        var authorNames = book
            .BookAuthors.Select(ba => $"{ba.Author.FirstName} {ba.Author.LastName}")
            .ToList();

        return string.Join(", ", authorNames);
    }
}
