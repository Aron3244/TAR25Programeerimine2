using System;

namespace ShopTARpe25.Core.Dto
{
    public class ImageViewModel
    {
        public Guid ImageId { get; set; }
        public string? FilePath { get; set; }
        public byte[]? ImageData { get; set; }
        public Guid? KindergartenId { get; set; }
    }
}