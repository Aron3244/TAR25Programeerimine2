
﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

namespace ShopTARpe25.ApplicationServices.Services
{
    // FileServices klass, mis teostab IFileServices liidese meetodid
    public class FileServices : IFileServices
    {
        // Privaatne andmebaasikonteksti muutuja päringute tegemiseks
        private readonly ShopTARpe25Context _context;

        // Konstruktor, mille kaudu antakse ette andmebaasi kontekst
        public FileServices(ShopTARpe25Context context)
        {
            // Omistab sissetulnud andmebaasikonteksti klassisisesele muutujale
            _context = context;
        }

        // Asünkroonne meetod pildifailide kettale ja baasi lisamiseks
        public async Task<List<FileToApi>> UploadFilesToDatabase(KindergartenDto dto, Kindergarten domain)
        {
            // Loob uue tühja nimekirja lisatud piltide tagastamiseks
            var result = new List<FileToApi>();

            // Kontrollib, kas kasutaja valis ankeedist vähemalt ühe faili
            if (dto.Files != null && dto.Files.Count > 0)
            {
                // Määrab sihtkausta tee: wwwroot/images/kindergarten
                string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "kindergarten");

                // Kontrollib, kas määratud kaust on juba kettal olemas
                if (!Directory.Exists(uploadDir))
                {
                    // Loob uue kausta kettale, kui seda veel ei eksisteeri
                    Directory.CreateDirectory(uploadDir);
                }

                // Käib läbi kõik kasutaja lisatud failid ükshaaval
                foreach (var file in dto.Files)
                {
                    // Kontrollib, et fail ei oleks tühi
                    if (file.Length > 0)
                    {
                        // Genereerib Guid koodi abil unikaalse failinime
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);

                        // Moodustab täieliku failitee serveri kettal
                        string filePath = Path.Combine(uploadDir, uniqueFileName);

                        // Avab failivoo faili kettale kirjutamiseks
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            // Kirjutab faili sisu asünkroonselt kettale
                            await file.CopyToAsync(fileStream);
                        }

                        // Loob uue andmebaasiobjekti pildi kirje jaoks
                        var fileToApi = new FileToApi
                        {
                            // Määrab pildikirjele unikaalse ID võtme
                            Id = Guid.NewGuid(),
                            // Salvestab pildifaili unikaalse nime
                            ImageTitle = uniqueFileName,
                            // Seob pildi vastava lasteaia identifikaatoriga
                            KindergartenId = domain.Id
                        };

                        // Lisab uue kirje andmebaasikonteksti
                        await _context.FileToApis.AddAsync(fileToApi);

                        // Lisab lisatud kirje tagastatavasse nimekirja
                        result.Add(fileToApi);
                    }
                }

                // Salvestab kõik uued pildikirjed lõplikult andmebaasi
                await _context.SaveChangesAsync();
            }

            // Tagastab baasi salvestatud piltide nimekirja
            return result;
        }

        // Asünkroonne meetod ühe kindla pildi kustutamiseks ID järgi
        public async Task<FileToApi> RemoveImage(Guid id)
        {
            // Otsib andmebaasist pilti vastavalt etteantud ID-le
            var image = await _context.FileToApis.FirstOrDefaultAsync(x => x.Id == id);

            // Kontrollib, kas pilt leiti baasist
            if (image == null)
            {
                // Tagastab nulli, kui vastavat pilti ei leitud
                return null;
            }

            // Määrab pildi täieliku asukohatee kettal
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "kindergarten", image.ImageTitle);

            // Kontrollib, kas pildifail eksisteerib reaalselt kettal
            if (File.Exists(filePath))
            {
                // Kustutab pildifaili serveri kettalt
                File.Delete(filePath);
            }

            // Eemaldab pildikirje andmebaasi kontekstist
            _context.FileToApis.Remove(image);

            // Salvestab kustutamise muudatuse andmebaasi
            await _context.SaveChangesAsync();

            // Tagastab kustutatud pildi objekti
            return image;
        }

        // Asünkroonne meetod kõigi ühe lasteaiaga seotud piltide kustutamiseks
        public async Task<List<FileToApi>> RemoveImagesFromDatabase(Guid kindergartenId)
        {
            // Otsib baasist välja kõik antud lasteaiale kuuluvad pildid
            var images = await _context.FileToApis
                .Where(x => x.KindergartenId == kindergartenId)
                .ToListAsync();

            // Käib tsükliga läbi kõik leitud pildid
            foreach (var image in images)
            {
                // Moodustab faili asukohatee serveri kettal
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "kindergarten", image.ImageTitle);

                // Kontrollib, kas fail eksisteerib serveri kettal
                if (File.Exists(filePath))
                {
                    // Kustutab pildifaili kettalt
                    File.Delete(filePath);
                }

                // Eemaldab pildikirje andmebaasi kontekstist
                _context.FileToApis.Remove(image);
            }

            // Salvestab piltide kustutamise muudatused andmebaasi
            await _context.SaveChangesAsync();

            // Tagastab kustutatud piltide nimekirja
            return images;
        }
    }
}

