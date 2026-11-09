using ShopTARpe25.Core.Dto;

namespace ShopTARpe25.Models.RealEstate
{
    public class RealEstateDelete
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
