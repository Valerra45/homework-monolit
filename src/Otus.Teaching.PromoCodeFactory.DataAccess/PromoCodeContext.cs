using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess
{
    public class PromoCodeContext : DbContext
    {
        public PromoCodeContext(DbContextOptions<PromoCodeContext> options)
            : base(options)
        {

        }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Preference> Preferences { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<PromoCode> PromoCodes { get; set; }

        public DbSet<Partner> Partners { get; set; }
    }
}
