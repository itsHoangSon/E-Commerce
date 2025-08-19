using BuildingBlock.EFCore;
using BuildingBlock.OS;
using BuildingBlock.Web;
using E_Commerce.Entities;
using MassTransit;
using MassTransit.RabbitMqTransport;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Data
{
    public class ECommerceDbContext : AppDbContextBase
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> dbContextOptions,
          ICurrentUserProvider currentUserProvider,
          IDateTimeProvider dateTimeProvider, IPublisher publisher) : base(dbContextOptions, currentUserProvider, dateTimeProvider, publisher)
        {
        }
        public ECommerceDbContext() { }
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(ServiceConstants.NAME);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>().HasQueryFilter(m => !m.IsDeleted);
            modelBuilder.Entity<Category>().HasQueryFilter(m => !m.IsDeleted);
            modelBuilder.Entity<Product>().HasQueryFilter(m => !m.IsDeleted);
            modelBuilder.Entity<ProductImage>().HasQueryFilter(m => !m.IsDeleted);

            modelBuilder.Entity<AppUser>()
                .Property(e => e.Id)
                .ValueGeneratedNever(); // Không auto increment
        }
    }
}
