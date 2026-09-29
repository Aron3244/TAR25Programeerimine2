using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseTask.Data;
using DatabaseTask.Core.Domain;


namespace DatabaseTask.Controllers
{
    public class KindergartenController : Controller
    {
        private readonly DatabaseTaskDbContext _context;

        public KindergartenController(DatabaseTaskDbContext context)
        {
            _context = context;
        }

      
        public async Task<IActionResult> Index()
        {
            var list = await _context.Kindergartens.ToListAsync();
            return View(list);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kindergarten = await _context.Kindergartens
                .FirstOrDefaultAsync(m => m.Id == id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            return View(kindergarten);
        }

        public IActionResult Create()
        {
            return View();
        }

    
        [HttpPost]
        public async Task<IActionResult> Create(Kindergarten kindergarten)
        {
            if (ModelState.IsValid)
            {
                kindergarten.CreatedAt = DateTime.UtcNow;
                kindergarten.UpdatedAt = DateTime.UtcNow;

                _context.Add(kindergarten);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kindergarten);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kindergarten = await _context.Kindergartens.FindAsync(id);
            if (kindergarten == null)
            {
                return NotFound();
            }
            return View(kindergarten);
        }

   
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Kindergarten kindergarten)
        {
            if (id != kindergarten.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.Kindergartens.FindAsync(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }

                    existing.GroupName = kindergarten.GroupName;
                    existing.ChildrenCount = kindergarten.ChildrenCount;
                    existing.KindergartenName = kindergarten.KindergartenName;
                    existing.TeacherName = kindergarten.TeacherName;
                    existing.UpdatedAt = DateTime.UtcNow;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Kindergartens.AnyAsync(e => e.Id == id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(kindergarten);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kindergarten = await _context.Kindergartens
                .FirstOrDefaultAsync(m => m.Id == id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            return View(kindergarten);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kindergarten = await _context.Kindergartens.FindAsync(id);
            if (kindergarten != null)
            {
                _context.Kindergartens.Remove(kindergarten);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}