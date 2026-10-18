using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

namespace ShopTARpe25.Controllers
{
    public class KindergartenController : Controller
    {
        private readonly ShopTARpe25Context _context;
        private readonly IKindergartenServices _kindergartenServices;
        private readonly IFileServices _fileServices;

        public KindergartenController(
            ShopTARpe25Context context,
            IKindergartenServices kindergartenServices,
            IFileServices fileServices)
        {
            _context = context;
            _kindergartenServices = kindergartenServices;
            _fileServices = fileServices;
        }

   
        private async Task<List<ImageViewModel>> LoadImages(Guid id)
        {
            return await _context.FileToApis
                .Where(x => x.KindergartenId == id)
                .Select(y => new ImageViewModel
                {
                    ImageId = y.Id,
                    FilePath = y.ImageTitle,
                    KindergartenId = y.KindergartenId
                }).ToListAsync();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _kindergartenServices.GetAllAsync();
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailsAsync(id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            var dto = new KindergartenDto
            {
                Id = kindergarten.Id,
                GroupName = kindergarten.GroupName,
                ChildrenCount = kindergarten.ChildrenCount,
                KindergartenName = kindergarten.KindergartenName,
                TeacherName = kindergarten.TeacherName,
                CreatedAt = kindergarten.CreatedAt,
                UpdatedAt = kindergarten.UpdatedAt,
                Image = await LoadImages(id)
            };

            return View(dto);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KindergartenDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _kindergartenServices.Create(dto);

                if (dto.Files != null && dto.Files.Count > 0)
                {
                    await _fileServices.UploadFilesToDatabase(dto, result);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailsAsync(id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            var dto = new KindergartenDto
            {
                Id = kindergarten.Id,
                GroupName = kindergarten.GroupName,
                ChildrenCount = kindergarten.ChildrenCount,
                KindergartenName = kindergarten.KindergartenName,
                TeacherName = kindergarten.TeacherName,
                CreatedAt = kindergarten.CreatedAt,
                UpdatedAt = kindergarten.UpdatedAt,
                Image = await LoadImages(id)
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, KindergartenDto dto)
        {
            if (id != dto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _kindergartenServices.Update(dto);

           
                if (dto.Files != null && dto.Files.Count > 0)
                {
                    await _fileServices.UploadFilesToDatabase(dto, result);
                }

                return RedirectToAction(nameof(Details), new { id });
            }

            dto.Image = await LoadImages(id);
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailsAsync(id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            var dto = new KindergartenDto
            {
                Id = kindergarten.Id,
                GroupName = kindergarten.GroupName,
                ChildrenCount = kindergarten.ChildrenCount,
                KindergartenName = kindergarten.KindergartenName,
                TeacherName = kindergarten.TeacherName,
                CreatedAt = kindergarten.CreatedAt,
                UpdatedAt = kindergarten.UpdatedAt,
                Image = await LoadImages(id)
            };

            return View(dto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
       
            await _fileServices.RemoveImagesFromDatabase(id);
            await _kindergartenServices.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveImage(Guid imageId, Guid kindergartenId, string returnTo = "Details")
        {
            await _fileServices.RemoveImage(imageId);

            if (returnTo != "Edit" && returnTo != "Details")
            {
                returnTo = "Details";
            }

            return RedirectToAction(returnTo, new { id = kindergartenId });
        }
        [HttpGet]
        public IActionResult Update(Guid id)
        {
            return RedirectToAction(nameof(Edit), new { id });
        }

    }
}