using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DemoExAgain.Entities
{

    public partial class DemoExContext : DbContext
    {
        public DemoExContext()
        {
            Database.EnsureCreated();
        }

        public DemoExContext(DbContextOptions<DemoExContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Manufacturer> Manufacturers { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderProduct> OrderProducts { get; set; }

        public virtual DbSet<PickupPoint> PickupPoints { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseMySql("user=root;server=localhost;database=DemoEx;password=bumblebeelion", ServerVersion.Parse("8.0.27-mysql"));
            
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("manufacturer");

                entity.Property(e => e.Name).HasColumnType("text");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("order");

                entity.HasIndex(e => e.PickupPointId, "order_pickuppoint_idx");

                entity.Property(e => e.ClientName).HasColumnType("text");
                entity.Property(e => e.DeliveryDate).HasColumnType("datetime");
                entity.Property(e => e.OrderDate).HasColumnType("datetime");
                entity.Property(e => e.Status).HasColumnType("enum('Новый','Завершен')");

                entity.HasOne(d => d.PickupPoint).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PickupPointId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("order_pickuppoint");
            });

            modelBuilder.Entity<OrderProduct>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("orderproduct");

                entity.HasIndex(e => e.ProductId, "orderproduct_product_idx");

                entity.HasOne(d => d.Order).WithMany(p => p.OrderProducts)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("orderproduct_order");

                entity.HasOne(d => d.Product).WithMany(p => p.OrderProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("orderproduct_product");
            });

            modelBuilder.Entity<PickupPoint>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("pickuppoint");

                entity.Property(e => e.Address).HasColumnType("text");
                entity.Property(e => e.Index).HasColumnType("text");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("product");

                entity.HasIndex(e => e.ManufacturerId, "product_manufacturer_idx");

                entity.Property(e => e.Cost).HasPrecision(10);
                entity.Property(e => e.Description).HasColumnType("text");
                entity.Property(e => e.Name).HasColumnType("text");
                entity.Property(e => e.Photo).HasColumnType("text");

                entity.HasOne(d => d.Manufacturer).WithMany(p => p.Products)
                    .HasForeignKey(d => d.ManufacturerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("product_manufacturer");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("role");

                entity.Property(e => e.Name).HasColumnType("text");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("user");

                entity.HasIndex(e => e.RoleId, "user_role_idx");

                entity.Property(e => e.Login).HasColumnType("text");
                entity.Property(e => e.Name).HasColumnType("text");
                entity.Property(e => e.Password).HasColumnType("text");
                entity.Property(e => e.Patronymic).HasColumnType("text");
                entity.Property(e => e.Surname).HasColumnType("text");

                entity.HasOne(d => d.Role).WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}