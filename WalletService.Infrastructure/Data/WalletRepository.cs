using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletService.Core.Domain.Entities;
using WalletService.Core.Interfaces;

namespace WalletService.Infrastructure.Data
{
    public class WalletRepository : IWalletRepository
    {
        private readonly WalletDbContext _dbContext;

        public WalletRepository(WalletDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Wallet> GetByUserIdAsync(Guid userId)
        {
            return await _dbContext.Wallets
                .Include(w => w.Accounts)
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }

        public async Task SaveAsync(Wallet wallet)
        {
            // For new entities (Id not in database)
            if (wallet.Id == Guid.Empty || !await _dbContext.Wallets.AnyAsync(w => w.Id == wallet.Id))
            {
                _dbContext.Wallets.Add(wallet);
            }
            else
            {
                // For existing entities
                if (_dbContext.Entry(wallet).State == EntityState.Detached)
                {
                    _dbContext.Wallets.Update(wallet);
                }
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
