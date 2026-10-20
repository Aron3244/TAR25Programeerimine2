using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class LibaryService
    {
        private readonly ShopTARpe25Context _context;

        public LibaryService(ShopTARpe25Context context)
        {
            _context = context;
        }

        public async Task AddBook(BookDto dto)
        {
            Book newBook = new();   
            
                newBook.Title = dto.Title;
                newBook.Author = dto.Author;
                newBook.PublishedYear = dto.PublishedYear;
                newBook.IsAvailable = true;

            await _context.Books.AddAsync(newBook);
            await _context.SaveChangesAsync();
        }
    }
}
