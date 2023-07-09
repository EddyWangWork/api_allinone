using demoAPI.Model.DS;
using demoAPI.Model.School;
using Microsoft.EntityFrameworkCore;

namespace demoAPI.Data.DS
{
    public class DSContext : DbContext
    {
        public DSContext(DbContextOptions<DSContext> options) : base(options)
        {
        }

        public DbSet<DSItem> DSItems { get; set; }
        public DbSet<DSItemSub> DSItemSubs { get; set; }
        public DbSet<TransProfile> DSAccounts { get; set; }
        public DbSet<DSType> DSTypes { get; set; }
        public DbSet<DSTransaction> DSTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DSItem>().ToTable("DSItem");
            modelBuilder.Entity<DSItemSub>().ToTable("DSItemSub");
            modelBuilder.Entity<TransProfile>().ToTable("DSAccount");
            modelBuilder.Entity<DSType>().ToTable("DSType");
            modelBuilder.Entity<DSTransaction>().ToTable("DSTransaction");
        }
    }
}
