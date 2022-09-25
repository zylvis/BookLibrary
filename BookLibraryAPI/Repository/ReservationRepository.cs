using BookLibraryAPI.Data;
using BookLibraryAPI.Models;
using BookLibraryAPI.Repository.IRepository;
using BookLibraryAPI.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookLibraryAPI.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _db;

        public ReservationRepository(ApplicationDbContext db)
        {
            _db = db;

        }
        public async Task CreateAsync(Reservation entity, string userId)
        {
            Book book = await _db.Books.FirstOrDefaultAsync(x => x.Id == entity.BookId);
            book.Reserved = true;
            _db.Books.Update(book);

            entity.Date = DateTime.Now;
            entity.UserId = userId;
            await _db.Reservations.AddAsync(entity);

            await SaveAsync();
        }

        public async Task<List<Reservation>> GetAllAsync(Expression<Func<Reservation, bool>>? filter = null)
        {
            IQueryable<Reservation> query = _db.Reservations;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<Reservation> GetAsync(Expression<Func<Reservation, bool>> filter = null, bool tracked = true)
        {
            IQueryable<Reservation> query = _db.Reservations;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(Reservation entity)
        {
            Book book = await _db.Books.FirstOrDefaultAsync(x => x.Id == entity.BookId);
            book.Reserved = false;
            _db.Books.Update(book);

            _db.Reservations.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
