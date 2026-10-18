using System;
using Microsoft.AspNetCore.Http;
namespace ShopTARpe25.Core.Dto
{
    public class KindergartenDto
    {
        public Guid? Id { get; set; }
        public string GroupName { get; set; }
        public int ChildrenCount { get; set; }
        public string KindergartenName { get; set; }
        public string TeacherName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<IFormFile>? Files { get; set; }

        public List<ImageViewModel>? Image { get; set; } = new List<ImageViewModel>();
    }
}