﻿using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.INFRASTRUCTURE.Repositories
{
    public interface ILocalRepository
    {
        Task<PaginatedList<LocalEntity>> GetUserLocalAsync(string appUserId, int pageNumber, int pageSize, string searchTerm);
        Task<LocalEntity> GetLocalByIdAsync(Guid id);
        Task<LocalEntity> AddLocalAsync(LocalEntity entity);
        Task<LocalEntity> UpdateLocalAsync(Guid localId, LocalEntity entity);
        Task<bool> DeleteLocalAsync(Guid localId);
    }
}
