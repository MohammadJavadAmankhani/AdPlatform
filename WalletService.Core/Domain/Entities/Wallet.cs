using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WalletService.Core.Domain.Events;

namespace WalletService.Core.Domain.Entities
{
    public class Wallet
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public List<Account> Accounts { get; private set; } = new();
        private readonly List<object> _domainEvents = new();

        public IReadOnlyList<object> DomainEvents => _domainEvents.AsReadOnly();

        public Wallet(Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
        }

        public void AddAccount(AccountType type, decimal initialBalance)
        {
            Accounts.Add(new Account(type, initialBalance));
        }

        public void Deduct(AccountType type, decimal amount)
        {
            var account = Accounts.FirstOrDefault(a => a.Type == type);
            if (account == null || account.Balance < amount)
                throw new InvalidOperationException("Insufficient balance");
            account.Deduct(amount);
            _domainEvents.Add(new WalletDeductedEvent(Id, UserId, type, amount));
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
   
}
