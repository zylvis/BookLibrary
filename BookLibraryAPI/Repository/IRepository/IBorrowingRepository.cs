using BookLibraryAPI.Models;
using System.Linq.Expressions;

namespace BookLibraryAPI.Repository.IRepository
{
    public interface IBorrowingRepository
    {
        Task<List<Borrowing>> GetAllAsync(Expression<Func<Borrowing, bool>>? filter = null);
        Task<Borrowing> GetAsync(Expression<Func<Borrowing, bool>> filter = null, bool tracked = true);
        Task CreateAsync(Borrowing entity, string userID);
        Task RemoveAsync(Borrowing entity);
        Task SaveAsync();
    }

}
