using System;
using System.Collections.Generic;
using System.Text;

namespace ShopTARpe25.Core.Domain
{
    public class FileToApi
    { 
        public Guid id { get; set; }
        public string? ExistingPath { get; set; }
        public Guid? KindergartenId { get; set; }
    }
}
