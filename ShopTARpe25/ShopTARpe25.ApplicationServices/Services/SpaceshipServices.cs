using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class SpaceshipServices : ISpaceshipServices
    {
        private readonly ShopTARpe25Context _context;

        public SpaceshipServices(ShopTARpe25Context context)
        {
            _context = context;
        }
        public async Task<Spaceship> Create(SpaceshipDto dto)
        {
            Spaceship domain = new();

            domain.Id = dto.Id;
            domain.Name = dto.Name;
            domain.Classification = dto.Classification;
            domain.BuiltDate = dto.BuiltDate;
            domain.Crew = dto.Crew;
            domain.EnginePower = dto.EnginePower;
            domain.CreatedAt = dto.CreatedAt;
            domain.ModifiedAt = dto.ModifiedAt;

            //siia tuleb kood, mis salvestab domaini
            //objekti andmebaasi
            //tuleb kasutada repositoryd, mus on defineeritud Core projektis
            //konstruktori kadu tuleb injectida repository
            await _context.Spaceships.AddAsync(domain);
            await _context.SaveChangesAsync();

            return domain;

        }
    }
}
