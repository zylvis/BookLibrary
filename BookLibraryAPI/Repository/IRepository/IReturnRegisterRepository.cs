using BookLibraryAPI.Models;
using System.Linq.Expressions;

namespace BookLibraryAPI.Repository.IRepository
{
    public interface IReturnRegisterRepository
    {
        Task<List<ReturnRegister>> GetAllAsync(Expression<Func<ReturnRegister, bool>>? filter = null);
        Task<ReturnRegister> GetAsync(Expression<Func<ReturnRegister, bool>> filter = null, bool tracked = true);
        Task CreateAsync(ReturnRegister entity);
        Task RemoveAsync(ReturnRegister entity);
        Task SaveAsync();
    }
}
