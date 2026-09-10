using Microsoft.AspNetCore.Mvc;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Models.Spaceship;
using ShopTARpe25.Core.ServiceInterface;



namespace ShopTARpe25.Controllers
{
    public class SpaceshipController : Controller
    {

        private readonly ISpaceshipServices _spaceshipService;
        

        public SpaceshipController
            (
            ISpaceshipServices _spaceshipService
            )
        {
            _spaceshipService = _spaceshipService;
        }

        public IActionResult Index()
        {
            return View();
        }

        //teha Data projekti alla ShopTARpe25Context nimega class
        //kui kasutaka kilikab Create nupp, siis see meetod käivitatakse
        //tagastab kasutajale vormi, kuhu saab sisestada andmeid
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        //kui oled teinud vormi, siis see  meetodd käivaitatakse
        //saadab andmeed serverisse, kus need salvestatakse andmebaasi
        [HttpPost]
        public async Task<IActionResult> Create(SpaceshipCreateViewModel vm)
        {
            //luua vaheinstants, mis sisaldab andmeid, mis on saadud vormist
            //need andmed tuleb edasi saata dto-sse, mis mõeldus andmebaasi salvestamiseks
            var dto = new SpaceshipDto
            {
                Name = vm.Name,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower,
                Classification = vm.Classification,
                BuiltDate = vm.BuiltDate

            };
            var result = await _spaceshipService.Create(dto);
           

            return RedirectToAction(nameof(Index));
        }
    }
}
