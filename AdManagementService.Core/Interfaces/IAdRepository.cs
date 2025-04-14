using AdManagementService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdManagementService.Core.Interfaces
{
    public interface IAdRepository
    {
        Task<Advertisement> GetByIdAsync(Guid id);
        Task<List<Advertisement>> GetByUserIdAsync(Guid userId);
        Task SaveAsync(Advertisement ad);
    }
}
