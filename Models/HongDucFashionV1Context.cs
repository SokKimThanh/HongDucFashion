using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HongDucFashion.Models
{
    public partial class HongDucFashionV1Context : DbContext
    {
        public HongDucFashionV1Context()
        {
        }

        public HongDucFashionV1Context(DbContextOptions<HongDucFashionV1Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Coupon> Coupons { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<InventoryTransaction> InventoryTransactions { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Promotion> Promotions { get; set; } = null!;
        public virtual DbSet<RoleAccount> RoleAccounts { get; set; } = null!;
        public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
        public virtual DbSet<UserAccount> UserAccounts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=HongDucFashionDb");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryName).HasMaxLength(100);

                entity.Property(e => e.Description).HasMaxLength(255);
            });

            modelBuilder.Entity<Coupon>(entity =>
            {
                entity.HasIndex(e => e.Code, "UQ_Coupons_Code")
                    .IsUnique();

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ExpiryDate).HasColumnType("date");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.Phone, "IX_Customer_Phone");

                entity.HasIndex(e => e.Email, "UQ_Customers_Email")
                    .IsUnique();

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.CustomerName).HasMaxLength(100);

                entity.Property(e => e.Email).HasMaxLength(150);

                entity.Property(e => e.Phone).HasMaxLength(20);
            });

            modelBuilder.Entity<InventoryTransaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId);

                entity.HasIndex(e => e.ProductId, "IX_InventoryTransactions_ProductId");

                entity.Property(e => e.Note).HasMaxLength(255);

                entity.Property(e => e.TransactionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TransactionType).HasMaxLength(50);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.InventoryTransactions)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_InventoryTransactions_Products");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasIndex(e => e.CouponId, "IX_Orders_CouponId");

                entity.HasIndex(e => e.CustomerId, "IX_Orders_CustomerId");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Coupon)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CouponId)
                    .HasConstraintName("FK_Orders_Coupons");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Orders_Customers");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasIndex(e => e.OrderId, "IX_OrderDetails_OrderId");

                entity.HasIndex(e => e.ProductId, "IX_OrderDetails_ProductId");

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderDetails_Orders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_OrderDetails_Products");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(e => e.CategoryId, "IX_Products_CategoryId");

                entity.HasIndex(e => e.ProductName, "IX_Products_ProductName");

                entity.HasIndex(e => e.SupplierId, "IX_Products_SupplierId");

                entity.Property(e => e.AvailableQuantity).HasDefaultValueSql("((0))");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProductName).HasMaxLength(100);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Products_Categories");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_Products_Suppliers");

                entity.HasMany(d => d.Promotions)
                    .WithMany(p => p.Products)
                    .UsingEntity<Dictionary<string, object>>(
                        "ProductPromotion",
                        l => l.HasOne<Promotion>().WithMany().HasForeignKey("PromotionId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ProductPromotions_Promotions"),
                        r => r.HasOne<Product>().WithMany().HasForeignKey("ProductId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ProductPromotions_Products"),
                        j =>
                        {
                            j.HasKey("ProductId", "PromotionId");

                            j.ToTable("ProductPromotions");

                            j.HasIndex(new[] { "ProductId" }, "IX_ProductPromotions_ProductId");

                            j.HasIndex(new[] { "PromotionId" }, "IX_ProductPromotions_PromotionId");
                        });
            });

            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.HasIndex(e => e.EndDate, "IX_Promotions_EndDate");

                entity.HasIndex(e => e.StartDate, "IX_Promotions_StartDate");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.DiscountPercent).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.PromotionName).HasMaxLength(100);

                entity.Property(e => e.StartDate).HasColumnType("date");
            });

            modelBuilder.Entity<RoleAccount>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("RoleAccount");

                entity.HasIndex(e => e.RoleName, "UQ_RoleAccount_RoleName")
                    .IsUnique();

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.Property(e => e.ContactInfo).HasMaxLength(255);

                entity.Property(e => e.SupplierName).HasMaxLength(100);
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("UserAccount");

                entity.HasIndex(e => e.Email, "UQ_UserAccount_Email")
                    .IsUnique();

                entity.Property(e => e.Email).HasMaxLength(150);

                entity.Property(e => e.PasswordHash).HasMaxLength(255);

                entity.Property(e => e.UserName).HasMaxLength(100);

                // Một UserAccount có thể liên kết với một Customer
                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.UserAccounts)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_UserAccount_Customers");

                // Một UserAccount có thể có nhiều RoleAccount
                // Một RoleAccount có thể gán cho nhiều UserAccount
                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserRoleAccount",
                        l => l.HasOne<RoleAccount>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_UserRoleAccount_RoleAccount"),
                        r => r.HasOne<UserAccount>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_UserRoleAccount_UserAccount"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("UserRoleAccount");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
