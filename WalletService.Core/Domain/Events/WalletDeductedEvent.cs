using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletService.Core.Domain.Entities;

namespace WalletService.Core.Domain.Events
{
    public class WalletDeductedEvent
    {
        public Guid WalletId { get; set; }
        public Guid UserId { get; set; }
        public AccountType AccountType { get; set; }
        public decimal Amount { get; set; }

        public WalletDeductedEvent(Guid walletId, Guid userId, AccountType accountType, decimal amount)
        {
            WalletId = walletId;
            UserId = userId;
            AccountType = accountType;
            Amount = amount;
        }
    }
}
