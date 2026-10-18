using System;
using System.Collections.Generic;
using System.Text;

namespace ShopTARpe25.Core.Dto
{
    public class FileToAptDto
    {
        public Guid id { get; set; }
        public string? ExistingPath { get; set; }
        public Guid? KindergartenId { get; set; }
    }
}
