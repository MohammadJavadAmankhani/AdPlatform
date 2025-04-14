using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using WalletService.Core.Domain.Entities;

namespace WalletService.Infrastructure.Data
{
    public class WalletDbContext : DbContext
    {
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wallet>()
                .HasMany(w => w.Accounts)
                .WithOne()
                .HasForeignKey("WalletId");

            modelBuilder.Entity<Account>()
                .Property(a => a.Balance)
                .HasColumnType("decimal(18,2)");
        }
    }
}
