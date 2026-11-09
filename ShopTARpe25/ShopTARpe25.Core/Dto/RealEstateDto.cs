using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopTARpe25.Core.Dto
{
    public class RealEstateDto
    {
        public Guid? Id { get; set; }
        public double? Area { get; set; }
        public string Aadress { get; set; }
        public int RoomNumber { get; set; }
        public string? BuildingType { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<IFormFile> Files { get; set; }
        public IEnumerable<FileToApiDto> fileToAptDtos { get; set; }
        = new List<FileToApiDto>();
        public DateTime? ModifiedAt { get; set; }
    }
}
