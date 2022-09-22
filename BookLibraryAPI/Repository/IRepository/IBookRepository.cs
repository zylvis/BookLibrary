using BookLibraryAPI.Models;
using System.Linq.Expressions;

namespace BookLibraryAPI.Repository.IRepository
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync(Expression<Func<Book, bool>>? filter = null);
        Task<List<Book>> SearchAllAsync(Expression<Func<Book, bool>>? filter = null);
        Task<Book> UpdateAsync(Book entity);
        Task<Book> GetAsync(Expression<Func<Book, bool>> filter = null, bool tracked = true);
        Task CreateAsync(Book entity);
        Task RemoveAsync(Book entity);
        Task SaveAsync();
    }
}
