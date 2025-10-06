using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Extensions;

namespace EMS.CORE.Interfaces
{
    public interface IAddressRepository
    {
        Task<PaginatedList<AddressEntity>> GetUserAddressesAsync(string appUserId, int pageNumber, int pageSize, string searchTerm);
        Task<IEnumerable<AddressEntity>> GetUserAddressesForTaskAsync(string appUserId, string searchTerm);
        Task<AddressEntity> GetAddressByIdAsync(Guid id);
        Task<AddressEntity> AddAddressAsync(AddressEntity entity);
        Task<AddressEntity> UpdateAddressAsync(Guid addressId, string appUserId, AddressEntity entity);
        Task<bool> DeleteAddressAsync(Guid addressId, string appUserId);
    }
}
