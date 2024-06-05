using Microsoft.EntityFrameworkCore;
using ThrivoHR.Domain.Common.Interfaces;

namespace ThrivoHR.Infrastructure.Persistence.Configurations
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IUnitOfWork
    {


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);

            // ConfigureModel(modelBuilder);

        }
        //private static void ConfigureModel(ModelBuilder modelBuilder)
        //{
        //    throw new NotSupportedException();
        //}
    }
}


