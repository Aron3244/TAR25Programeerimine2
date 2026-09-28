

namespace ShopTARpe25.Core.Dto
{
    public class FileToAptDto
    {
        public Guid Id { get; set; }
        //see muutuja hakkab nätama, kus asub meie file
        public string? ExistingFilePath { get; set; }
        public Guid? SpaceshipId { get; set; }
    }
}
