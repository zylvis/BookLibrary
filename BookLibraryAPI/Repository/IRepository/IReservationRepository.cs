using BookLibraryAPI.Models;
using System.Linq.Expressions;

namespace BookLibraryAPI.Repository.IRepository
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetAllAsync(Expression<Func<Reservation, bool>>? filter = null);
        Task<Reservation> GetAsync(Expression<Func<Reservation, bool>> filter = null, bool tracked = true);
        Task CreateAsync(Reservation entity, string userId);
        Task RemoveAsync(Reservation entity);
        Task SaveAsync();
    }
}
