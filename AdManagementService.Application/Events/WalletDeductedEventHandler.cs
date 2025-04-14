using AdManagementService.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletService.Core.Domain.Events;
using WalletService.Core.Interfaces;

namespace AdManagementService.Application.Events
{
    public class WalletDeductedEventHandler
    {
        private readonly IAdRepository _adRepo;
        private readonly IWalletClient _walletClient;
        private readonly IEventBus _eventBus;

        public WalletDeductedEventHandler(IAdRepository adRepo, IWalletClient walletClient, IEventBus eventBus)
        {
            _adRepo = adRepo;
            _walletClient = walletClient;
            _eventBus = eventBus;
        }

        public async Task Handle(WalletDeductedEvent @event)
        {
            Console.WriteLine($"Handling WalletDeductedEvent: UserId={@event.UserId}, Amount={@event.Amount}");

            var ads = await _adRepo.GetByUserIdAsync(@event.UserId);
            foreach (var ad in ads)
            {
                var balance = await _walletClient.GetBalanceAsync(@event.UserId, @event.AccountType);
                ad.UpdateStatus(balance);
                await _adRepo.SaveAsync(ad);

                foreach (var domainEvent in ad.DomainEvents)
                {
                    await _eventBus.PublishAsync(domainEvent);
                }
                ad.ClearDomainEvents();
            }
        }
    }
}
