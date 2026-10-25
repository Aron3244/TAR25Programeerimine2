using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;

namespace ShopTARpe25.Controllers
{
    public class LibaryController : Controller
    {
        private readonly IBookServices _bookServices;

        public LibaryController(IBookServices bookServices)
        {
            _bookServices = bookServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? year, string? title)
        {
            if (year.HasValue)
            {
                return View(await _bookServices.GetBooksByYear(year.Value));
            }

            if (!string.IsNullOrWhiteSpace(title))
            {
                var book = await _bookServices.GetBookByTitle(title);
                var list = new List<Book>();

                if (book != null)
                {
                    list.Add(book);
                }

                return View(list);
            }

            return View(await _bookServices.GetAllBooks());
        }

        [HttpGet]
        public async Task<IActionResult> Loaned()
        {
            return View("Index", await _bookServices.GetCurrentlyLoanedBooks());
        }

        [HttpGet]
        public IActionResult Create()
        {
            BookDto dto = new();

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookDto dto)
        {
            await _bookServices.Create(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var book = await _bookServices.GetBookById(id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var book = await _bookServices.GetBookById(id);

            if (book == null)
            {
                return NotFound();
            }

            BookDto dto = new();

            dto.Id = book.Id;
            dto.Title = book.Title;
            dto.Author = book.Author;
            dto.PublishedYear = book.PublishedYear;

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(BookDto dto)
        {
            await _bookServices.Update(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookServices.GetBookById(id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(int id)
        {
            await _bookServices.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult CreateReader()
        {
            ReaderDto dto = new();

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReader(ReaderDto dto)
        {
            await _bookServices.CreateReader(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> LoanBook()
        {
            ViewBag.Books = new SelectList(await _bookServices.GetAvailableBooks(), "Id", "Title");
            ViewBag.Readers = new SelectList(await _bookServices.GetAllReaders(), "Id", "FullName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoanBook(int bookId, int readerId)
        {
            await _bookServices.LoanBook(bookId, readerId);

            return RedirectToAction(nameof(ReaderLoans), new { readerId });
        }

        [HttpGet]
        public async Task<IActionResult> ReaderLoans(int? readerId)
        {
            ViewBag.ReaderId = readerId;

            if (readerId == null)
            {
                return View(new List<Loan>());
            }

            return View(await _bookServices.GetLoansByReaderId(readerId.Value));
        }

        [HttpPost]
        public async Task<IActionResult> ReturnBook(int loanId, int readerId)
        {
            await _bookServices.ReturnBook(loanId);

            return RedirectToAction(nameof(ReaderLoans), new { readerId });
        }
    }
}