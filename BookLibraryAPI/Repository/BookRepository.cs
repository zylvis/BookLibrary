using BookLibraryAPI.Data;
using BookLibraryAPI.Models;
using BookLibraryAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace BookLibraryAPI.Repository
{
    public class BookRepository :  IBookRepository
    {
        private readonly ApplicationDbContext _db;

        public BookRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Book>> GetAllAsync(Expression<Func<Book, bool>>? filter = null)
        {
            IQueryable<Book> query = _db.Books;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<List<Book>> SearchAllAsync(Expression<Func<Book, bool>>? filter = null)
        {
            IQueryable<Book> query = _db.Books;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<Book> GetAsync(Expression<Func<Book, bool>> filter = null, bool tracked = true)
        {
            IQueryable<Book> query = _db.Books;
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


        public async Task CreateAsync(Book entity)
        {
            entity.SearchColumn = $"{entity.Author}{entity.Title}{entity.Publisher}{entity.ISBN}{entity.Year}{entity.Genre}{entity.AvailableStatus}".ToLower();
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;
            entity.AvailableStatus = true;
            await _db.Books.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<Book> UpdateAsync(Book entity)
        {
            entity.SearchColumn = $"{entity.Author}{entity.Title}{entity.Publisher}{entity.ISBN}{entity.Year}{entity.Genre}{entity.AvailableStatus}".ToLower();
            entity.UpdatedDate = DateTime.Now;
            _db.Books.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }



        public async Task RemoveAsync(Book entity)
        {
            _db.Books.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
