using Microsoft.AspNetCore.Mvc;
using ShopTARpe25.ApplicationServices.Services;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using ShopTARpe25.Models.RealEstate;
using ShopTARpe25.Models.Spaceship;

namespace ShopTARpe25.Controllers
{
    public class RealEstateController
    {
        private readonly IRealEstateServices _realEstateServices;
        private readonly ShopTARpe25Context _context;
        private readonly IFileServices _fileService;
        //teha constructor et saaks kasutada teenust, mis on
        //defineeritud ISpaceshipServices liideses
        //lisage Context
        public RealEstateController
            (
                IFileServices fileService,
                IRealEstateServices realEstateServices,
                ShopTARpe25Context context
            )
        {
            _realEstateServices = realEstateServices;
            _context = context;
            _fileService = fileService;

        }


        public IActionResult Index()
        {
            //loome vaheinstantsi domaini ja viewModeli vahel.
            var result = _context.RealEstates
                .Select(x => new RealEstateIndex
                {
                    Id = x.Id,
                    Area = x.Area,
                    Aadress = x.Aadress,
                    RoomNumber = x.RoomNumber,
                    BuildingType = x.BuildingType,
                    ModifiedAt = x.ModifiedAt
                });

            return View(result);
        }

        //kui kasutaja klikib "Create" nuppu, siis see meetod käivitatakse
        //tagastab kasutajale vormi, kuhu saab sisestada andmed
        [HttpGet]
        public IActionResult Create()
        {
            RealeEstateCreateUpdate vm = new();

            return View("CreateUpdate", vm);
        }

        //kui oled teinud vormi, siis see meetod käivitatakse
        //saadab andmed serverisse, kus need salvestatakse andmebaasi
        [HttpPost]
        public async Task<IActionResult> Create(RealeEstateCreateUpdate vm)
        {
            //luua vaheinstants, mis sisaldab andmeid, mis on saadud vormist
            //need andmed tuleb edasi saata dto-sse, mis on mõeldud andmebaasi salvestamiseks

            var dto = new SpaceshipDto
            {
                Id = vm.Id,
                Name = vm.Name,
                Classification = vm.Classification,
                BuiltDate = vm.BuiltDate,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower,
                Files = vm.Files,
                FileToApiDtos = vm.Image
                    .Select(file => new FileToApiDto
                    {
                        Id = file.ImageId,
                        ExistingFilePath = file.FilePath,
                        SpaceshipId = file.SpaceshipId
                    }).ToArray()
            };

            //kutsuda teenuse meetodit, mis salvestab andmed andmebaasi
            var result = await _spaceshipService.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        //tuleb teha Details meetod
        //see kutsub v'lja interfacest service meetodi

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            //meetodi kutsumine interfacest
            var spaceship = await _spaceshipService.DetailsAsync(id);

            //veakäsitlus
            //suunab vaatele NotFound, kui andmeid ei ole
            if (spaceship == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
                .Where(x => x.SpaceshipId == id)
                .Select(y => new ImageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageId = y.Id,
                    SpaceshipId = y.SpaceshipId
                }).ToArrayAsync();

            //tuleb teha viewModel ja see siin välja kutsuda
            //ära map-ida vm ja domain
            var vm = new SpaceshipDetailsViewModel();

            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.Classification = spaceship.Classification;
            vm.BuiltDate = spaceship.BuiltDate;
            vm.Crew = spaceship.Crew;
            vm.EnginePower = spaceship.EnginePower;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;
            vm.Images.AddRange(images);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var spaceship = await _spaceshipService.DetailsAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
           .Where(x => x.SpaceshipId == id)
           .Select(y => new ImageViewModel
           {
               FilePath = y.ExistingFilePath,
               ImageId = y.Id,
               SpaceshipId = y.SpaceshipId
           }).ToArrayAsync();

            var vm = new RealeEstateCreateUpdate();

            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.Classification = spaceship.Classification;
            vm.BuiltDate = spaceship.BuiltDate;
            vm.Crew = spaceship.Crew;
            vm.EnginePower = spaceship.EnginePower;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;
            vm.Image.AddRange(images);

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SpaceshipUpdateViewModel vm)
        {
            var dto = new SpaceshipDto()
            {
                Id = vm.Id,
                Name = vm.Name,
                Classification = vm.Classification,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower,
                BuiltDate = vm.BuiltDate,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt,
                Files = vm.Files,
                FileToApiDtos = vm.Images
                    .Select(file => new FileToApiDto
                    {
                        Id = file.ImageId,
                        ExistingFilePath = file.FilePath,
                        SpaceshipId = file.SpaceshipId
                    }).ToArray()
            };

            var result = await _spaceshipService.Update(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var spaceship = await _spaceshipService.DetailsAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }

            //kirjuta kood, mis aitab näidata pilte, mis 
            //kuuluvad konkreetsele kosmoselaevale
            //kui hakkan kustutama, siis saan teada, mida ma kustutan 

            var images = await _context.FileToApis
           .Where(x => x.SpaceshipId == id)
           .Select(y => new ImageViewModel
           {
               FilePath = /*"~/multipleFileUpload/" + */y.ExistingFilePath,
               ImageId = y.Id,
               SpaceshipId = y.SpaceshipId
           }).ToArrayAsync();

            var vm = new SpaceshipDeleteViewModel();

            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.Classification = spaceship.Classification;
            vm.BuiltDate = spaceship.BuiltDate;
            vm.Crew = spaceship.Crew;
            vm.EnginePower = spaceship.EnginePower;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;
            vm.Images.AddRange(images);


            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var result = await _spaceshipService.Delete(id);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveImage(ImageViewModel vm)
        {
            //tuleb ühendada dto ja viewModel, et saaks kasutrada teenuse ,meetodit.
            var dto = new FileToApiDto()
            {
                Id = vm.ImageId
            };


            //kutsuda teenus meetodi, mis kasutab pildi andmebaasist
            var image = await _fileService.RemoveImageFromApi(dto);

            if (image == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));


        }


    }
}
