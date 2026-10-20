using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopTARpe25.Core.Dto
{
    public class BookDto
    {

        public int Id { get; set; }
        public string Title { get; set; } 
        public string Author { get; set; } 
        public int PublishedYear { get; set; }
        public bool IsAvailable { get; set; }

        public List<IFormFile>? Files { get; set;}
    }
}
