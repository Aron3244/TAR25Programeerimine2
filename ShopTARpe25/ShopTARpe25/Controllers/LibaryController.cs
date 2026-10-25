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
                return View(await _bookServices.GetBooksByYear(year.Value));

            if (!string.IsNullOrWhiteSpace(title))
            {
                var book = await _bookServices.GetBookByTitle(title);
                return View(book == null ? new List<Book>() : new List<Book> { book });
            }

            return View(await _bookServices.GetAllBooks());
        }

        [HttpGet]
        public async Task<IActionResult> Loaned()  
        {
            return View("Index", await _bookServices.GetCurrentlyLoanedBooks());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var book = await _bookServices.GetBookById(id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateBookTitle(int id)
        {
            var book = await _bookServices.GetBookById(id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBookTitle(int id, string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
            {
                TempData["Error"] = "Pealkiri ei tohi olla tühi.";
                return RedirectToAction(nameof(UpdateBookTitle), new { id });
            }

            var updated = await _bookServices.UpdateBookTitle(id, newTitle);
            if (updated == null) return NotFound();
            return RedirectToAction(nameof(Index));
        }

       
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookServices.GetBookById(id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _bookServices.GetBookById(id);
            if (book == null) return NotFound();

            if (!await _bookServices.Delete(id))
                TempData["Error"] = "Raamatut ei saa kustutada, sest see on praegu laenutuses. Tagasta see enne.";

            return RedirectToAction(nameof(Index));
        }

      
        [HttpGet]
        public IActionResult CreateReader() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReader(ReaderDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
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

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> LoanBook(int bookId, int readerId)
        {
            var loan = await _bookServices.LoanBook(bookId, readerId);
            if (loan == null)
            {
                TempData["Error"] = "Laenutamine ebaõnnestus.";
                return RedirectToAction(nameof(LoanBook));
            }
            return RedirectToAction(nameof(ReaderLoans), new { readerId });
        }

     
        [HttpGet]
        public async Task<IActionResult> ReaderLoans(int? readerId)
        {
            ViewBag.ReaderId = readerId;
            if (!readerId.HasValue) return View(new List<Loan>());
            return View(await _bookServices.GetLoansByReaderId(readerId.Value));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnBook(int loanId, int readerId)
        {
            var loan = await _bookServices.ReturnBook(loanId);
            if (loan == null) TempData["Error"] = "Tagastamine ebaõnnestus.";
            return RedirectToAction(nameof(ReaderLoans), new { readerId });
        }
    }
}