using ShopTARpe25.Core.Dto;

namespace ShopTARpe25.Models.RealEstate
{
    public class RealEstateImages
    {
        public Guid ImageId { get; set; }
        public string? FilePath { get; set; }
        public Guid? RealEstateId { get; set; }
        public List<IFormFile> Files { get; set; }
        public IEnumerable<FileToApiDto> fileToAptDtos { get; set; }
        = new List<FileToApiDto>();
    }
}
