using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class LibaryService : IBookServices
    {
        private readonly ShopTARpe25Context _context;

        public LibaryService(ShopTARpe25Context context)
        {
            _context = context;
        }

        public async Task<Book> Create(BookDto dto)
        {
            Book newBook = new()
            {
                Title = dto.Title,
                Author = dto.Author,
                PublishedYear = dto.PublishedYear,
                IsAvailable = true
            };

            await _context.Books.AddAsync(newBook);
            await _context.SaveChangesAsync();
            return newBook;
        }

        public async Task<Reader> CreateReader(ReaderDto dto)
        {
            Reader newReader = new()
            {
                FullName = dto.FullName,
                Email = dto.Email
            };

            await _context.Readers.AddAsync(newReader);
            await _context.SaveChangesAsync();
            return newReader;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByYear(int year)
        {
            return await _context.Books
                .Where(b => b.PublishedYear == year)
                .ToListAsync();
        }

        public async Task<Book?> GetBookByTitle(string title)
        {
            return await _context.Books
                .FirstOrDefaultAsync(b => b.Title.ToLower().Contains(title.ToLower()));
        }

        public async Task<IEnumerable<Book>> GetCurrentlyLoanedBooks()
        {
            return await _context.Books
                .Where(b => b.IsAvailable == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetLoansByReaderId(int readerId)
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Where(l => l.ReaderId == readerId)
                .ToListAsync();
        }

        public async Task<Book?> UpdateBookTitle(int id, string newTitle)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null) return null;

            book.Title = newTitle;
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Loan?> ReturnBook(int loanId)
        {
            var loan = await _context.Loans.Include(l => l.Book).FirstOrDefaultAsync(l => l.Id == loanId);
            if (loan == null) return null;

            loan.ReturnDate = DateTime.Now;
            if (loan.Book != null)
            {
                loan.Book.IsAvailable = true;
            }

            _context.Loans.Update(loan);
            await _context.SaveChangesAsync();
            return loan;
        }

        public async Task<bool> Delete(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null) return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}