using BookLibraryAPI.Data;
using BookLibraryAPI.Models;
using BookLibraryAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookLibraryAPI.Repository
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private readonly ApplicationDbContext _db;

        public BookRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
       
        public async Task<Book> UpdateAsync(Book entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.Books.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
