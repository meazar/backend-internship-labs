using System.Collections.Concurrent; // to support multiple threads safely
using LMS.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace LMS.Infrastructure.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly LMSDbContext _context;
    private readonly ConcurrentDictionary<Type, object> _repositories;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(LMSDbContext context)
    {
        _context = context;
        _repositories = new ConcurrentDictionary<Type, object>();
    }

    public IGenericRepository<T> Repository<T>()
        where T : class
    {
        return (IGenericRepository<T>)
            _repositories.GetOrAdd(typeof(T), type => new GenericRepository<T>(_context));
    }

    public async Task<int> CompleteAsync()
    {
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("An error occurred while saving to the database.", ex);
        }
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            await _transaction!.CommitAsync();
        }
        finally
        {
            await _transaction!.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
