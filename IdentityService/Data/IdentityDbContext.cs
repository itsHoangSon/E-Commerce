using BuildingBlock.EFCore;
using BuildingBlock.OS;
using BuildingBlock.Web;
using IdentityService.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Data
{
    public class IdentityDbContext : AppDbContextBase
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> dbContextOptions,
          ICurrentUserProvider currentUserProvider,
          IDateTimeProvider dateTimeProvider, IPublisher publisher) : base(dbContextOptions, currentUserProvider, dateTimeProvider, publisher)
        {
        }
        public IdentityDbContext() { }
        public DbSet<AppUser> AppUser { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("data source=localhost;initial catalog=OttCore_Identity;persist security info=True;user id=SA;password=12345678;multipleactiveresultsets=True;TrustServerCertificate=true;");
        //        //optionsBuilder.UseSqlServer("Data Source=172.16.1.118;Initial Catalog=OttCore_Delivery;Persist Security Info=True;User ID=sa;Password=sa@123;MultipleActiveResultSets=True;TrustServerCertificate=True");
        //    }
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(ServiceConstants.NAME);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>().HasQueryFilter(m => !m.IsDeleted);

        }
    }
}
