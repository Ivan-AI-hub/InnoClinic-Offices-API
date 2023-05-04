using Microsoft.EntityFrameworkCore;
using OfficesAPI.DAL.EntityConfiguration;
using OfficesAPI.Domain;

namespace OfficesAPI.DAL
{
    public class OfficesContext : DbContext
    {
        public OfficesContext(DbContextOptions<OfficesContext> options) : base(options)
        {
        }

        public DbSet<Office> Offices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OfficeConfigurator());
        }
    }
}
