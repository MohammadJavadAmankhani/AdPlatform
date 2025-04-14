using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletService.Core.Domain.Entities;

namespace WalletService.Core.Interfaces
{
    public interface IWalletRepository
    {
        Task<Wallet> GetByUserIdAsync(Guid userId);
        Task SaveAsync(Wallet wallet);
    }
}
