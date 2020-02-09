using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GroupJoinMVC.Models.Entities
{
    public partial class GroupJoinDBContext : DbContext
    {
        public GroupJoinDBContext()
        {
        }

        public GroupJoinDBContext(DbContextOptions<GroupJoinDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Products2Stores> Products2Stores { get; set; }
        public virtual DbSet<Rating> Rating { get; set; }
        public virtual DbSet<Stores> Stores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=GroupJoinDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.ToTable("Products", "gj");

                entity.HasIndex(e => new { e.Name, e.Brand })
                    .HasName("UQ__Products__08DEF0EB74419015")
                    .IsUnique();

                entity.Property(e => e.Brand).HasMaxLength(100);

                entity.Property(e => e.ImgUrl).HasColumnName("ImgURL");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Products2Stores>(entity =>
            {
                entity.ToTable("Products2Stores", "gj");

                entity.HasIndex(e => new { e.ProductId, e.StoreId })
                    .HasName("UQ__Products__B7B4E9E2DBAB3B9A")
                    .IsUnique();

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Products2Stores)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Products2__Produ__5629CD9C");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Products2Stores)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Products2__Store__5535A963");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.ToTable("Rating", "gj");

                entity.HasIndex(e => new { e.UserId, e.ProductId })
                    .HasName("UQ__Rating__DCC8002118A77C80")
                    .IsUnique();

                entity.Property(e => e.Rating1).HasColumnName("Rating");

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Rating)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Rating__ProductI__3B75D760");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Rating)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Rating__UserId__3A81B327");
            });

            modelBuilder.Entity<Stores>(entity =>
            {
                entity.ToTable("Stores", "gj");

                entity.HasIndex(e => e.StoreName)
                    .HasName("UQ__Stores__520DB652BDD2CFB5")
                    .IsUnique();

                entity.Property(e => e.StoreName).IsRequired();
            });
        }
    }
}
