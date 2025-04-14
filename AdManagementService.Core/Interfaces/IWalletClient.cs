using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletService.Core.Domain.Entities;

namespace AdManagementService.Core.Interfaces
{
    public interface IWalletClient
    {
        Task<decimal> GetBalanceAsync(Guid userId, AccountType accountType);
    }
}
