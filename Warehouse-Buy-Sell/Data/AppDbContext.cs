using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Warehouse_Buy_Sell.Models;
namespace Warehouse_Buy_Sell.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseItems> PurchaseItems { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<InternalMovement> InternalMovements { get; set; }
        public DbSet<InternalMovementItem> InternalMovementItems { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Supplier
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasIndex(e => e.PersonalNumber).IsUnique();
            });

            // Customer
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.PersonalNumber).IsUnique();
               
            });

           
            // Purchase
            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.HasOne(p => p.warehouse)
                    .WithMany(w => w.purchases)
                    .HasForeignKey(p => p.WarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.supplier)
                    .WithMany(s => s.purchase)
                    .HasForeignKey(p => p.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);

               
            });

            // Purchase Item
            modelBuilder.Entity<PurchaseItems>(entity =>
            {
                entity.HasOne(pi => pi.purchase)
                    .WithMany(p => p.purchaseItems)
                    .HasForeignKey(pi => pi.PurchaseId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pi => pi.product)
                    .WithMany(p => p.purchaseItems)
                    .HasForeignKey(pi => pi.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Sale
            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasOne(s => s.warehouse)
                    .WithMany(w => w.sales)
                    .HasForeignKey(s => s.WarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.customer)
                    .WithMany(c => c.sale)
                    .HasForeignKey(s => s.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                
            });

            // Sale Item
            modelBuilder.Entity<SaleItem>(entity =>
            {
                entity.HasOne(si => si.sale)
                    .WithMany(s => s.saleItems)
                    .HasForeignKey(si => si.SaleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(si => si.product)
                    .WithMany(p => p.saleItems)
                    .HasForeignKey(si => si.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Internal Movement
            modelBuilder.Entity<InternalMovement>(entity =>
            {
                entity.HasOne(im => im.fromWarehouse)
                    .WithMany(w => w.MovementsFrom)
                    .HasForeignKey(im => im.FromWarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(im => im.toWarehouse)
                    .WithMany(w => w.MovementsTo)
                    .HasForeignKey(im => im.ToWarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);

               
            });

            // Internal Movement Item
            modelBuilder.Entity<InternalMovementItem>(entity =>
            {
                entity.HasOne(imi => imi.internalMovement)
                    .WithMany(im => im.internalMovementItems)
                    .HasForeignKey(imi => imi.MovementId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(imi => imi.product)
                    .WithMany(p => p.internalMovementItems)
                    .HasForeignKey(imi => imi.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Inventory
            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasIndex(e => new { e.WarehouseId, e.ProductId }).IsUnique();

                entity.HasOne(i => i.warehouse)
                    .WithMany(w => w.Inventories)
                    .HasForeignKey(i => i.WarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(i => i.product)
                    .WithMany(p => p.inventories)
                    .HasForeignKey(i => i.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

               
            });

            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
               
            });
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Supplier || e.Entity is Customer ||
                           e.Entity is Product || e.Entity is Warehouse ||
                           e.Entity is Purchase || e.Entity is Sale ||
                           e.Entity is InternalMovement || e.Entity is User);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                }
            }
        }
    }
}
    

