using System;
using BackendFoodOrder.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BackendFoodOrder.Models
{
    public partial class FoodOrderContext : DbContext
    {
        public FoodOrderContext()
        {
        }

        public FoodOrderContext(DbContextOptions<FoodOrderContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
/*#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.*/
                optionsBuilder.UseSqlServer("Server=tcp:foodorder-db.cchocdri8xz6.us-east-1.rds.amazonaws.com;Database=FoodOrderDB1;User ID=admin2;Password=adminadmin;Trusted_Connection=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryId");

                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Desc).IsRequired();
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Cart");

                entity.Property(e => e.CartId).HasColumnName("CartId");

                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Quantity).IsRequired();

                entity.Property(e => e.TotalAmount)
                    .IsRequired()
                    .HasMaxLength(50);

            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.UserId).HasColumnName("UserId");

            });

            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.ToTable("OrderDetails");

                entity.Property(e => e.OrderId).HasColumnName("OrderId");

                entity.Property(e => e.ProductId).HasColumnName("ProductId");

            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.ProductId).HasColumnName("ProductId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("productName");

                entity.Property(e => e.Desc)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("productDesc");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Stock).HasColumnName("stock");

                entity.Property(e => e.Category).HasColumnName("category");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnName("image");
               
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserLevel)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
