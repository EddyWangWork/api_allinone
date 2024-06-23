using demoAPI.Model;
using demoAPI.Model.DS;
using demoAPI.Model.Kanbans;
using demoAPI.Model.TodlistsDone;
using demoAPI.Model.Trip;
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
        public DbSet<Todolist> Todolists { get; set; }
        public DbSet<TodolistDone> TodolistsDone { get; set; }

        public DbSet<TripDetailType> TripDetailTypes { get; set; }
        public DbSet<TripDetail> TripDetails { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Kanban> Kanbans { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<ShopType> ShopTypes { get; set; }
        public DbSet<ShopDiary> ShopDiaries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DSItem>().ToTable("DSItem");
            modelBuilder.Entity<DSItemSub>().ToTable("DSItemSub");
            modelBuilder.Entity<DSAccount>().ToTable("DSAccount");
            modelBuilder.Entity<DSType>().ToTable("DSType");
            modelBuilder.Entity<DSTransaction>().ToTable("DSTransaction");
            modelBuilder.Entity<Member>().ToTable("Member");
            modelBuilder.Entity<Todolist>().ToTable("Todolist");
            modelBuilder.Entity<TodolistDone>().ToTable("TodolistDone");
            modelBuilder.Entity<TripDetailType>().ToTable("TripDetailType");
            modelBuilder.Entity<TripDetail>().ToTable("TripDetail");
            modelBuilder.Entity<Trip>().ToTable("Trip");
            modelBuilder.Entity<Kanban>().ToTable("Kanban");
            modelBuilder.Entity<Shop>().ToTable("Shop");
            modelBuilder.Entity<ShopType>().ToTable("ShopType");
            modelBuilder.Entity<ShopDiary>().ToTable("ShopDiary");

            modelBuilder.Entity<DSItem>()
            .HasIndex(x => new { x.MemberID, x.Name })
            .IsUnique(true);

            modelBuilder.Entity<DSItemSub>()
            .HasIndex(x => new { x.DSItemID, x.Name })
            .IsUnique(true);
        }
    }
}
