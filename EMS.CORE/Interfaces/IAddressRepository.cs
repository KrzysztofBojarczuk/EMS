using EMS.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.CORE.Interfaces
{
    public interface IAddressRepository
    {
        Task<IEnumerable<AddressEntity>> GetUserAddressesAsync(string appUserId, string searchTerm);
        Task<AddressEntity> GetAddressByIdAsync(Guid id);
        Task<AddressEntity> AddAddressAsync(AddressEntity entity);
        Task<AddressEntity> UpdateAddressAsync(Guid addressId, AddressEntity entity);
        Task<bool> DeleteAddressAsync(Guid addressId);
    }
}
