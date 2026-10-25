using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

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
            Book book = new();

            book.Title = dto.Title;
            book.Author = dto.Author;
            book.PublishedYear = dto.PublishedYear;
            book.IsAvailable = true;

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return book;
        }

        public async Task<Reader> CreateReader(ReaderDto dto)
        {
            Reader reader = new();

            reader.FullName = dto.FullName;
            reader.Email = dto.Email;

            await _context.Readers.AddAsync(reader);
            await _context.SaveChangesAsync();

            return reader;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAvailableBooks()
        {
            return await _context.Books.Where(x => x.IsAvailable == true).ToListAsync();
        }

        public async Task<IEnumerable<Reader>> GetAllReaders()
        {
            return await _context.Readers.ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByYear(int year)
        {
            return await _context.Books.Where(x => x.PublishedYear == year).ToListAsync();
        }

        public async Task<Book?> GetBookByTitle(string title)
        {
            return await _context.Books.FirstOrDefaultAsync(x => x.Title.Contains(title));
        }

        public async Task<Book?> GetBookById(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Book>> GetCurrentlyLoanedBooks()
        {
            return await _context.Books.Where(x => x.IsAvailable == false).ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetLoansByReaderId(int readerId)
        {
            return await _context.Loans
                .Include(x => x.Book)
                .Where(x => x.ReaderId == readerId)
                .ToListAsync();
        }

        public async Task<Book?> Update(BookDto dto)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (book == null)
            {
                return null;
            }

            book.Title = dto.Title;
            book.Author = dto.Author;
            book.PublishedYear = dto.PublishedYear;

            await _context.SaveChangesAsync();

            return book;
        }

        public async Task<Loan?> LoanBook(int bookId, int readerId)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == bookId);
            var reader = await _context.Readers.FirstOrDefaultAsync(x => x.Id == readerId);

            if (book == null || reader == null || book.IsAvailable == false)
            {
                return null;
            }

            Loan loan = new();

            loan.BookId = bookId;
            loan.ReaderId = readerId;
            loan.LoanDate = DateTime.Now;

            book.IsAvailable = false;

            await _context.Loans.AddAsync(loan);
            await _context.SaveChangesAsync();

            return loan;
        }

        public async Task<Loan?> ReturnBook(int loanId)
        {
            var loan = await _context.Loans
                .Include(x => x.Book)
                .FirstOrDefaultAsync(x => x.Id == loanId);

            if (loan == null || loan.ReturnDate != null)
            {
                return null;
            }

            loan.ReturnDate = DateTime.Now;
            loan.Book.IsAvailable = true;

            await _context.SaveChangesAsync();

            return loan;
        }

        public async Task<bool> Delete(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (book == null || book.IsAvailable == false)
            {
                return false;
            }

            var loans = await _context.Loans.Where(x => x.BookId == id).ToListAsync();

            _context.Loans.RemoveRange(loans);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}