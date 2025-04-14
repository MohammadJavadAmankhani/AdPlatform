using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletService.Core.Domain.Entities;
using WalletService.Core.Interfaces;

namespace WalletService.Application.Queries
{
    public class GetWalletBalanceQuery : IRequest<decimal>
    {
        public Guid UserId { get; set; }
        public AccountType AccountType { get; set; }
    }

    public class GetWalletBalanceQueryHandler : IRequestHandler<GetWalletBalanceQuery, decimal>
    {
        private readonly IWalletRepository _walletRepo;

        public GetWalletBalanceQueryHandler(IWalletRepository walletRepo)
        {
            _walletRepo = walletRepo;
        }

        public async Task<decimal> Handle(GetWalletBalanceQuery request, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepo.GetByUserIdAsync(request.UserId);
            if (wallet == null)
                throw new InvalidOperationException("Wallet not found");

            var account = wallet.Accounts.FirstOrDefault(a => a.Type == request.AccountType);
            return account?.Balance ?? 0;
        }
    }
}
