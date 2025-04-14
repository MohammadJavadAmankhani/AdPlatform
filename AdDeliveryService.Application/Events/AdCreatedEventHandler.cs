using AdDeliveryService.Core.Domain.Entities;
using AdDeliveryService.Core.Interfaces;
using AdManagementService.Core.Domain.Events;

namespace AdDeliveryService.Application.Events
{
    public class AdCreatedEventHandler
    {
        private readonly IAdDeliveryRepository _adRepo;

        public AdCreatedEventHandler(IAdDeliveryRepository adRepo)
        {
            _adRepo = adRepo;
        }

        public async Task Handle(AdCreatedEvent @event)
        {
            if (@event == null || @event.AdId == Guid.Empty)
            {
                Console.WriteLine("Invalid AdCreatedEvent: Event or AdId is invalid.");
                return;
            }

            var existingAd = await _adRepo.GetByAdIdAsync(@event.AdId);
            if (existingAd != null)
            {
                Console.WriteLine($"Ad already exists in Cosmos DB: AdId={@event.AdId}");
                return;
            }

            var ad = new AdDelivery(@event.AdId, "AAA");
            
            await _adRepo.SaveAsync(ad);
            Console.WriteLine($"Saved ad to Cosmos DB: AdId={@event.AdId}, IsActive={@event.IsActive}");
        }
    }
}