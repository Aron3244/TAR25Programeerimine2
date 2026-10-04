using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class KindergartenServices : IKindergartenServices
    {
        private readonly ShopTARpe25Context _context;

        public KindergartenServices(ShopTARpe25Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Kindergarten>> GetAllAsync()
        {
            return await _context.Kindergartens.ToListAsync();
        }

        public async Task<Kindergarten> DetailsAsync(Guid id)
        {
            return await _context.Kindergartens.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Kindergarten> Create(KindergartenDto dto)
        {
            Kindergarten domain = new Kindergarten
            {
                Id = Guid.NewGuid(),
                GroupName = dto.GroupName,
                ChildrenCount = dto.ChildrenCount,
                KindergartenName = dto.KindergartenName,
                TeacherName = dto.TeacherName,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _context.Kindergartens.AddAsync(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<Kindergarten> Update(KindergartenDto dto)
        {
            var domain = await _context.Kindergartens.FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (domain == null) return null;

            domain.GroupName = dto.GroupName;
            domain.ChildrenCount = dto.ChildrenCount;
            domain.KindergartenName = dto.KindergartenName;
            domain.TeacherName = dto.TeacherName;
            domain.UpdatedAt = DateTime.Now;

            _context.Kindergartens.Update(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<Kindergarten> Delete(Guid id)
        {
            var domain = await _context.Kindergartens.FirstOrDefaultAsync(x => x.Id == id);

            if (domain == null) return null;

            _context.Kindergartens.Remove(domain);
            await _context.SaveChangesAsync();

            return domain;
        }
    }
}