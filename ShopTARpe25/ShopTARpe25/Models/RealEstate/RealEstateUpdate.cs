using ShopTARpe25.Core.Dto;

namespace ShopTARpe25.Models.RealEstate
{
    public class RealEstateUpdate
    {
        public Guid? Id { get; set; }
        public double? Area { get; set; }
        public string Aadress { get; set; }
        public int RoomNumber { get; set; }
        public string? BuildingType { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<IFormFile> Files { get; set; }
        public List<ImageViewModel> Image { get; set; }
       = new List<ImageViewModel>();
        public DateTime? ModifiedAt { get; set; }
    }
}
