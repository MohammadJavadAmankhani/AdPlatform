using AdManagementService.Core.Domain.Entities;
using AdManagementService.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletService.Core.Domain.Entities;

namespace AdManagementService.Infrastructure.Data
{
    public class AdRepository : IAdRepository
    {
        private readonly AdDbContext _dbContext;

        public AdRepository(AdDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Advertisement> GetByIdAsync(Guid id)
        {
            return await _dbContext.Advertisements.FindAsync(id);
        }

        public async Task<List<Advertisement>> GetByUserIdAsync(Guid userId)
        {
            return await _dbContext.Advertisements
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task SaveAsync(Advertisement ad)
        {
            // For new entities (Id not in database)
            if (ad.Id == Guid.Empty || !await _dbContext.Advertisements.AnyAsync(w => w.Id == ad.Id))
            {
                _dbContext.Advertisements.Add(ad);
            }
            else
            {
                // For existing entities
                if (_dbContext.Entry(ad).State == EntityState.Detached)
                    _dbContext.Advertisements.Update(ad);
            }

            await _dbContext.SaveChangesAsync();
        }

    }
}
