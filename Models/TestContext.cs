using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace POC.Models
{
    public partial class TestContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public TestContext()
        {
        }

        public TestContext(DbContextOptions<TestContext> options, IConfiguration configuration)
            : base(options)
        {
            this._configuration = configuration;
        }

        public virtual DbSet<Orde> Orde { get; set; }
        public virtual DbSet<Ordes> Ordes { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Products> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Orde>(entity =>
            {
                entity.HasKey(e => e.Noa);

                entity.ToTable("orde");

                entity.Property(e => e.Noa)
                    .HasColumnName("noa")
                    .HasMaxLength(100)
                    .ValueGeneratedNever();

                entity.Property(e => e.Addr).HasColumnName("addr");

                entity.Property(e => e.Cust)
                    .HasColumnName("cust")
                    .HasMaxLength(200);

                entity.Property(e => e.Datea)
                    .HasColumnName("datea")
                    .HasMaxLength(100);

                entity.Property(e => e.Discount).HasColumnName("discount");

                entity.Property(e => e.Enda).HasColumnName("enda");

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasMaxLength(100);

                entity.Property(e => e.Memo).HasColumnName("memo");

                entity.Property(e => e.Money).HasColumnName("money");

                entity.Property(e => e.Odate)
                    .HasColumnName("odate")
                    .HasMaxLength(100);

                entity.Property(e => e.Total).HasColumnName("total");

                entity.Property(e => e.Trantype)
                    .HasColumnName("trantype")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Ordes>(entity =>
            {
                entity.HasKey(e => new { e.Noa, e.Noq });

                entity.ToTable("ordes");

                entity.Property(e => e.Noa)
                    .HasColumnName("noa")
                    .HasMaxLength(100);

                entity.Property(e => e.Noq)
                    .HasColumnName("noq")
                    .HasMaxLength(100);

                entity.Property(e => e.Flavor)
                    .HasColumnName("flavor")
                    .HasMaxLength(100);

                entity.Property(e => e.Memo).HasColumnName("memo");

                entity.Property(e => e.Mount).HasColumnName("mount");

                entity.Property(e => e.Pno)
                    .HasColumnName("pno")
                    .HasMaxLength(100);

                entity.Property(e => e.Product)
                    .HasColumnName("product")
                    .HasMaxLength(200);

                entity.Property(e => e.Sprice).HasColumnName("sprice");

                entity.Property(e => e.Total).HasColumnName("total");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Noa);

                entity.ToTable("product");

                entity.Property(e => e.Noa)
                    .HasColumnName("noa")
                    .HasMaxLength(100)
                    .ValueGeneratedNever();

                entity.Property(e => e.Product1).HasColumnName("product");

                entity.Property(e => e.Typea)
                    .HasColumnName("typea")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => new { e.Noa, e.Noq });

                entity.ToTable("products");

                entity.Property(e => e.Noa)
                    .HasColumnName("noa")
                    .HasMaxLength(100);

                entity.Property(e => e.Noq)
                    .HasColumnName("noq")
                    .HasMaxLength(100);

                entity.Property(e => e.Flavor)
                    .HasColumnName("flavor")
                    .HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Scount).HasColumnName("scount");
            });
        }
    }
}
