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
    public class CreateWalletCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public AccountType AccountType { get; set; }
        public decimal InitialBalance { get; set; }
    }

    public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, Unit>
    {
        private readonly IWalletRepository _walletRepo;

        public CreateWalletCommandHandler(IWalletRepository walletRepo)
        {
            _walletRepo = walletRepo;
        }

        public async Task<Unit> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            var wallet = new Wallet(request.UserId);
            wallet.AddAccount(request.AccountType, request.InitialBalance);

            await _walletRepo.SaveAsync(wallet);
            return Unit.Value;
        }
    }
}
