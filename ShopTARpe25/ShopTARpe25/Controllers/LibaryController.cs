using Microsoft.AspNetCore.Mvc;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;

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
        public async Task<IActionResult> Index(int? year)
        {
            if (year.HasValue)
            {
                var filteredBooks = await _bookServices.GetBooksByYear(year.Value);
                return View(filteredBooks);
            }

            
            var loanedBooks = await _bookServices.GetCurrentlyLoanedBooks();
            return View(loanedBooks);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var result = await _bookServices.Create(dto);

            if (result != null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(dto);
        }
    }
}
