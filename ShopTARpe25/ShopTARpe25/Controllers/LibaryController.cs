using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Index(int? year)
        {
            if (year.HasValue)
            {
                var filteredBooks = await _bookServices.GetBooksByYear(year.Value);
                return View(filteredBooks);
            }

            var allBooks = await _bookServices.GetAllBooks();
            return View(allBooks);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(BookDto dto)
        {
            if (ModelState.IsValid)
            {
                await _bookServices.Create(dto);
                return RedirectToAction(nameof(Index));
            }

            return View(dto);
        }
        

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookServices.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var books = await _bookServices.GetAllBooks();
            var book = books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateBookTitle(int id)
        {
            var books = await _bookServices.GetAllBooks();
            var book = books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBookTitle(int id, string newTitle)
        {
            var updatedBook = await _bookServices.UpdateBookTitle(id, newTitle);

            if (updatedBook == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}