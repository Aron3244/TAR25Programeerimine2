using Microsoft.AspNetCore.Mvc;
using ShopTARpe25.Models.Spaceship;

namespace ShopTARpe25.Controllers
{
    public class SpaceshipController : Controller
    {
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
            return RedirectToAction(nameof(Index));
        }
    }
}
