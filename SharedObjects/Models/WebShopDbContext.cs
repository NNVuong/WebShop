using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedObjects.ValueObjects;

namespace SharedObjects.Models
{
    public class WebShopDbContext : IdentityDbContext<AppUser,AppRole,string>
    {
        public WebShopDbContext(DbContextOptions<WebShopDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<TransactStatus> TransactStatuses { get; set; }
        public virtual DbSet<VCategory> VCategory { get; set; }
        public virtual DbSet<VProduct> VProduct { get; set; }
        public virtual DbSet<VSales> VSales { get; set; }
        public virtual DbSet<VProductImage> VProductImage { get; set; }
        public virtual DbSet<VProductCategory> VProductCategory { get; set; }
        public virtual DbSet<VOrder> VOrder { get; set; }
        public virtual DbSet<VOrderDetail> VOrderDetail { get; set; }
        public virtual DbSet<VTransactStatus> VTransactStatus { get; set; }
        public virtual DbSet<VNew> VNew { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=localhost,1433;Initial Catalog=WebShopDbContext;User ID=sa;Password=Password.1");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CatId);

                entity.Property(e => e.CatId).HasColumnName("CatID");

                entity.Property(e => e.Alias).HasMaxLength(250);

                entity.Property(e => e.CatName).HasMaxLength(250);
                entity.HasIndex(e => e.CatName).IsUnique();

                entity.Property(e => e.Cover).HasMaxLength(255);

                entity.Property(e => e.MetaDesc).HasMaxLength(250);

                entity.Property(e => e.MetaKey).HasMaxLength(250);

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.Thumb).HasMaxLength(250);

                entity.Property(e => e.Title).HasMaxLength(250);
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.HasKey(e => e.PostId)
                    .HasName("PK__News__AA1260385DCE55A4");

                entity.Property(e => e.PostId).HasColumnName("PostID");

                entity.Property(e => e.Alias).HasMaxLength(255);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IsHot)
                    .IsRequired()
                    .HasColumnName("isHot")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IsNewFeed)
                    .IsRequired()
                    .HasColumnName("isNewFeed")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.MetaDesc).HasMaxLength(255);

                entity.Property(e => e.MetaKey).HasMaxLength(255);

                entity.Property(e => e.Scontents)
                    .HasMaxLength(255)
                    .HasColumnName("SContents");

                entity.Property(e => e.Thumb).HasMaxLength(255);

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(x => x.AppUser).WithMany(x => x.News).HasForeignKey(x => x.UserId);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.ShipDate).HasColumnType("datetime");

                entity.Property(e => e.TotalMoney).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TransactStatusId).HasColumnName("TransactStatusID");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.TransactStatus)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.TransactStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__Transact__4222D4EF");
                entity.HasOne(x => x.AppUser).WithMany(x => x.Orders).HasForeignKey(x => x.UserId);
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Discount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.TotalMoney).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__OrderDeta__Order__45F365D3");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__OrderDeta__Produ__46E78A0C");
            });

            modelBuilder.Entity<Page>(entity =>
            {
                entity.Property(e => e.PageId).HasColumnName("PageID");

                entity.Property(e => e.Alias).HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.MetaDesc).HasMaxLength(250);

                entity.Property(e => e.MetaKey).HasMaxLength(250);

                entity.Property(e => e.PageName).HasMaxLength(250);

                entity.Property(e => e.Published)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Thumb).HasMaxLength(250);

                entity.Property(e => e.Title).HasMaxLength(250);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(x => x.AppUser).WithMany(x => x.Pages).HasForeignKey(x => x.UserId);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Alias).HasMaxLength(255);

                entity.Property(e => e.CatId).HasColumnName("CatID");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.Property(e => e.MetaDesc).HasMaxLength(255);

                entity.Property(e => e.MetaKey).HasMaxLength(255);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(255);
                entity.HasIndex(e => e.ProductName).IsUnique();

                entity.Property(e => e.ShortDesc).HasMaxLength(255);

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.Property(e => e.Video).HasMaxLength(255);

                entity.HasOne(d => d.Cat)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CatId)
                    .HasConstraintName("FK_Products_Categories");
                entity.Property(e => e.UnitsInStock)
                    .HasColumnName("UnitsInStock")
                    .HasDefaultValueSql("((0))");
                entity.Property(x => x.Avatar).HasMaxLength(255);
            });

            modelBuilder.Entity<TransactStatus>(entity =>
            {
                entity.ToTable("TransactStatus");

                entity.Property(e => e.TransactStatusId).HasColumnName("TransactStatusID");

                entity.Property(e => e.Status).HasMaxLength(50);
            });
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.Property(x => x.FullName).IsRequired().HasMaxLength(200);
                entity.Property(x => x.Address).HasMaxLength(200);
                entity.Property(x => x.Avatar).HasMaxLength(200);
            });

            modelBuilder.Entity<AppRole>(entity =>
            {
                entity.Property(x => x.Description).HasMaxLength(200);
            });
            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.Property(x => x.ImageName).HasMaxLength(250);
                entity.HasOne(p => p.Product)
                      .WithMany(p => p.ProductImages)
                      .HasForeignKey(p => p.ProductId)
                      .HasConstraintName("FK_ProductImages_Products");
            });
        }
    }
}
