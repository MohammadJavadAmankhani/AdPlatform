using AdManagementService.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AdManagementService.Infrastructure.Data
{
    public class AdDbContext : DbContext
    {
        public DbSet<Advertisement> Advertisements { get; set; }

        public AdDbContext(DbContextOptions<AdDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Advertisement>()
                .Property(a => a.Budget)
                .HasColumnType("decimal(18,2)");
        }
    }
}
