﻿using BookLibraryAPI.Data;
using BookLibraryAPI.Models;
using BookLibraryAPI.Repository.IRepository;
using BookLibraryAPI.Utility;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookLibraryAPI.Repository
{
    public class BorrowingRepository : IBorrowingRepository
    {
        private readonly ApplicationDbContext _db;

        public BorrowingRepository(ApplicationDbContext db)
        {
            _db = db; 
        }

        public async Task<List<Borrowing>> GetAllAsync(Expression<Func<Borrowing, bool>>? filter = null)
        {
            IQueryable<Borrowing> query = _db.Borrowings;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }


        public async Task<Borrowing> GetAsync(Expression<Func<Borrowing, bool>> filter = null, bool tracked = true)
        {
            IQueryable<Borrowing> query = _db.Borrowings;
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


        public async Task CreateAsync(Borrowing entity, string userID)
        {

            Book book = await _db.Books.FirstOrDefaultAsync(x => x.Id == entity.BookID);
            bool bookReserved = book.Reserved;
            bool reservedByUser = await _db.Reservations.FirstOrDefaultAsync(x => x.UserId == entity.UserID && x.BookId == entity.BookID) != null;

            Reservation reservation = await _db.Reservations.FirstOrDefaultAsync(x => x.BookId == entity.BookID);
            if (reservation != null)
            {
                _db.Reservations.Remove(reservation);
            }

            book.Reserved = false;
            book.AvailableStatus = SD.Unavailable;
            _db.Books.Update(book);

            entity.Date = DateTime.Now;
            entity.UserID = userID;
            await _db.Borrowings.AddAsync(entity);

            await SaveAsync();
        }


        public async Task RemoveAsync(Borrowing entity)
        {
            Book book = await _db.Books.FirstOrDefaultAsync(x => x.Id == entity.BookID);
            book.AvailableStatus = SD.Available;
            _db.Books.Update(book);

            _db.Borrowings.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
