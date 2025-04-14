using AdManagementService.Core.Domain.Entities;
using AdManagementService.Core.Domain.Events;
using AdManagementService.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletService.Core.Interfaces;

namespace AdManagementService.Application.Commands
{
    public class CreateAdCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public decimal Budget { get; set; }
    }

    public class CreateAdCommandHandler : IRequestHandler<CreateAdCommand, Unit>
    {
        private readonly IAdRepository _adRepo;
        private readonly ICacheService _cacheService;
        private readonly IEventBus _eventBus;

        public CreateAdCommandHandler(IAdRepository adRepo, ICacheService cacheService, IEventBus eventBus)
        {
            _adRepo = adRepo;
            _cacheService = cacheService;
            _eventBus = eventBus;
        }

        public async Task<Unit> Handle(CreateAdCommand request, CancellationToken cancellationToken)
        {
            var ad = new Advertisement(request.UserId, request.Budget);
            await _adRepo.SaveAsync(ad);

            await _cacheService.SetAsync($"ad:{ad.Id}", ad, TimeSpan.FromHours(24));
            Console.WriteLine($"Cached ad in Redis: ad:{ad.Id}");

            await _eventBus.PublishAsync(new AdCreatedEvent(ad.Id, ad.UserId, ad.Budget, ad.IsActive));

            return Unit.Value;
        }
    }
}
