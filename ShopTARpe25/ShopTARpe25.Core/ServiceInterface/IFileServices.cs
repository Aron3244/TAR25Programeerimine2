using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopTARpe25.Core.ServiceInterface
{
    public interface IFileServices
    {
        Task<List<FileToApi>> UploadFilesToDatabase(KindergartenDto dto, Kindergarten domain);
        Task<FileToApi> RemoveImage(Guid id);
        Task<List<FileToApi>> RemoveImagesFromDatabase(Guid kindergartenId);
    }
}

