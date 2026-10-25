using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopTARpe25.Core.ServiceInterface
{
    public interface IBookServices
    {
        Task<Book> Create(BookDto dto);
        Task<Reader> CreateReader(ReaderDto dto);
        Task<IEnumerable<Book>> GetAllBooks();
        Task<IEnumerable<Book>> GetAvailableBooks();
        Task<IEnumerable<Reader>> GetAllReaders();
        Task<IEnumerable<Book>> GetBooksByYear(int year);
        Task<Book?> GetBookByTitle(string title);
        Task<Book?> GetBookById(int id);
        Task<IEnumerable<Book>> GetCurrentlyLoanedBooks();
        Task<IEnumerable<Loan>> GetLoansByReaderId(int readerId);
        Task<Book?> Update(BookDto dto);
        Task<Loan?> LoanBook(int bookId, int readerId);
        Task<Loan?> ReturnBook(int loanId);
        Task<bool> Delete(int id);
    }
}

