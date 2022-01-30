using Otus.Teaching.PromoCodeFactory.Core.Abstractions;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly PromoCodeContext _context;

        public DbInitializer(PromoCodeContext context)
        {
            _context = context;
        }

        public void Initialize()
        {
            _context.Database.EnsureDeleted();

            _context.Database.EnsureCreated();

            if (_context.Roles.Any())
            {
                return;
            }

            _context.AddRange(FakeDataFactory.Employees);
            _context.SaveChanges();

            _context.AddRange(FakeDataFactory.Customers);
            _context.SaveChanges();

            _context.AddRange(FakeDataFactory.Partners);
            _context.SaveChanges();

            foreach (var p in FakeDataFactory.Preferences)
            {
                if (!_context.Preferences.Contains(p))
                {
                    _context.Preferences.Add(p);
                }
            }
            _context.SaveChanges();

            foreach (var r in FakeDataFactory.Roles)
            {
                if (!_context.Roles.Contains(r))
                {
                    _context.Roles.Add(r);
                }
            }

            _context.SaveChanges();
        }
    }
}
