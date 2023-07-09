﻿using demoAPI.Model.DS;
using demoAPI.Model;
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
        public DbSet<DSAccount> DSAccounts { get; set; }
        public DbSet<DSType> DSTypes { get; set; }
        public DbSet<DSTransaction> DSTransactions { get; set; }
        public DbSet<Member> Members { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DSItem>().ToTable("DSItem");
            modelBuilder.Entity<DSItemSub>().ToTable("DSItemSub");
            modelBuilder.Entity<DSAccount>().ToTable("DSAccount");
            modelBuilder.Entity<DSType>().ToTable("DSType");
            modelBuilder.Entity<DSTransaction>().ToTable("DSTransaction");
            modelBuilder.Entity<Member>().ToTable("Member");
        }
    }
}
