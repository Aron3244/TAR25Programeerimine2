using System;
using System.Collections.Generic;
using System.Text;

namespace ShopTARpe25.Core.Dto
{
    ///Dto class vahendab andmeid controlleri ja service classide vahel.

    internal class SpaceshipDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Classification { get; set; } = string.Empty;
        public DateTime? BuildDate { get; set; }
        public int? Crew { get; set; }
        public int? EnginePower { get; set; }

        public DateTime? CreateAt { get; set; }
        public DateTime? ModifieAt { get; set; }
    }
}
