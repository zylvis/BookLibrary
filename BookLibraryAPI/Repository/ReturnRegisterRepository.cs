using BookLibraryAPI.Data;
using BookLibraryAPI.Models;
using BookLibraryAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookLibraryAPI.Repository
{
    public class ReturnRegisterRepository : IReturnRegisterRepository
    {
        private readonly ApplicationDbContext _db;

        public ReturnRegisterRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task CreateAsync(ReturnRegister entity)
        {
            entity.Date = DateTime.Now;
            await _db.ReturnRegisters.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<List<ReturnRegister>> GetAllAsync(Expression<Func<ReturnRegister, bool>>? filter = null)
        {
            IQueryable<ReturnRegister> query = _db.ReturnRegisters;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<ReturnRegister> GetAsync(Expression<Func<ReturnRegister, bool>> filter = null, bool tracked = true)
        {
            IQueryable<ReturnRegister> query = _db.ReturnRegisters;
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

        public async Task RemoveAsync(ReturnRegister entity)
        {
            _db.ReturnRegisters.Remove(entity); 
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}