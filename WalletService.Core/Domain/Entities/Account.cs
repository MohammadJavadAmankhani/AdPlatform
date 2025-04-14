using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletService.Core.Domain.Entities
{
    public class Account
    {
        public Guid Id { get; private set; }
        public AccountType Type { get; private set; }
        public decimal Balance { get; private set; }

        public Account()
        {

        }

        public Account(AccountType type, decimal initialBalance)
        {
            Id = Guid.NewGuid();
            Type = type;
            Balance = initialBalance;
        }

        public void Deduct(decimal amount)
        {
            Balance -= amount;
        }
    }

    public enum AccountType
    {
        Cash,
        Points,
        Discount
    }
}
