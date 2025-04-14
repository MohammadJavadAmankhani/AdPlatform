using AdDeliveryService.Core.Interfaces;
using AdManagementService.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdDeliveryService.Application.Events
{
    public class AdPausedEventHandler
    {
        private readonly IAdDeliveryRepository _adRepo;

        public AdPausedEventHandler(IAdDeliveryRepository adRepo)
        {
            _adRepo = adRepo;
        }

        public async Task Handle(AdPausedEvent @event)
        {
            var ad = await _adRepo.GetByAdIdAsync(@event.AdId);
            if (ad != null)
            {
                ad.Pause();
                await _adRepo.SaveAsync(ad);
            }
        }
    }
}
