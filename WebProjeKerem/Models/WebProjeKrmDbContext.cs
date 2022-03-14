using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebProjeKerem.Models
{
    public partial class WebProjeKrmDbContext : DbContext
    {
        public WebProjeKrmDbContext()
        {
        }

        public WebProjeKrmDbContext(DbContextOptions<WebProjeKrmDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<OrderLines> OrderLines { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WebProjeKrmDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderLines>(entity =>
            {
                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderLines)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_dbo.OrderLines_dbo.Orders_OrderId");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderLines)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_dbo.OrderLines_dbo.Products_ProductId");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.Property(e => e.OrderDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_dbo.Products_dbo.Categories_CategoryId");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.IsRole)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.RePassword).IsRequired();

                entity.Property(e => e.Username).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
