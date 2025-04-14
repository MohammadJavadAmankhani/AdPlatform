using AdDeliveryService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdDeliveryService.Core.Interfaces
{

    public interface IAdDeliveryRepository
    {
        Task<List<AdDelivery>> GetActiveAdsAsync();
        Task<AdDelivery> GetByAdIdAsync(Guid adId);
        Task SaveAsync(AdDelivery ad);
    }
}
