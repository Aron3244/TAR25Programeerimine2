using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopTARpe25.Core.ServiceInterface
{
    public interface IBookServices
    {
        Task<Book> Create(BookDto dto);
        Task<IEnumerable<Book>> GetAllBooks();
        Task<IEnumerable<Book>> GetBooksByYear(int year);
        Task<Book?> GetBookByTitle(string title);
        Task<IEnumerable<Book>> GetCurrentlyLoanedBooks();
        Task<IEnumerable<Loan>> GetLoansByReaderId(int readerId);
        Task<Book?> UpdateBookTitle(int id, string newTitle);
        Task<Loan?> ReturnBook(int loanId);
        Task<bool> Delete(int id);
    }
}
