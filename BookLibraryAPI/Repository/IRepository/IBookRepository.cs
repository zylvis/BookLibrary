using BookLibraryAPI.Models;
using System.Linq.Expressions;

namespace BookLibraryAPI.Repository.IRepository
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book> UpdateAsync(Book entity);

    }
}
