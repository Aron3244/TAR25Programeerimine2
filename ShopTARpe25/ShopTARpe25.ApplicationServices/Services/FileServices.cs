using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using System.Net;


namespace ShopTARpe25.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly ShopTARpe25Context _context;
        private readonly IHostEnvironment _webHost;

        public FileServices
          (
             
             ShopTARpe25Context context,
            IHostEnvironment webHost
          )
        {

            _context = context;
            _webHost = webHost;
            
        }

        public void FilesToAPI(SpaceshipDto dto, Spaceship domain)
        {
            if(dto.Files != null && dto.Files.Count > 0)

                //kui ei ole wwwroot-s multipleFileUpload directoryt
            {
                if (!Directory.Exists(_webHost.ContentRootPath + "\\wwwroot\\multipleFiledUpload\\"))
                {
                    //tee directory wwrooti alla
                    Directory.CreateDirectory(_webHost.ContentRootPath + "\\wwwroot\\multipleFiledUpload\\");

                }
                
                foreach(var file in dto.Files)
                {
                    //meil on vaja treha muutuja nimega uploadFolder.
                    //sinna muutuja tha on vaja Path kombineerida

                    string uploadFolder = Path.Combine(_webHost.ContentRootPath, "wwwroot", "multipleFileUpload");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.Name;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                        //domaini thea FileToApi
                        FileToApi
                    }

                }
            }
        }
    }
}
