using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletService.Core.Domain.Entities;
using WalletService.Core.Interfaces;

namespace WalletService.Application.Commands
{
    public class DeductWalletCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public AccountType AccountType { get; set; }
        public decimal Amount { get; set; }
    }

    public class DeductWalletCommandHandler : IRequestHandler<DeductWalletCommand, Unit>
    {
        private readonly IWalletRepository _walletRepo;
        private readonly IEventBus _eventBus;

        public DeductWalletCommandHandler(IWalletRepository walletRepo, IEventBus eventBus)
        {
            _walletRepo = walletRepo;
            _eventBus = eventBus;
        }

        public async Task<Unit> Handle(DeductWalletCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepo.GetByUserIdAsync(request.UserId);
            if (wallet == null)
                throw new InvalidOperationException("Wallet not found");

            wallet.Deduct(request.AccountType, request.Amount);
            await _walletRepo.SaveAsync(wallet);

            foreach (var @event in wallet.DomainEvents)
            {
                await _eventBus.PublishAsync(@event);
            }
            wallet.ClearDomainEvents();
            return Unit.Value;
        }
    }
}
