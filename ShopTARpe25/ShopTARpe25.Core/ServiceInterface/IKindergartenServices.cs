
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;

namespace ShopTARpe25.Core.ServiceInterface
{
    public interface IKindergartenServices
    {
        Task<IEnumerable<Kindergarten>> GetAllAsync();
        Task<Kindergarten> DetailsAsync(Guid id);
        Task<Kindergarten> Create(KindergartenDto dto);
        Task<Kindergarten> Update(KindergartenDto dto);
        Task<Kindergarten> Delete(Guid id);
    }
}