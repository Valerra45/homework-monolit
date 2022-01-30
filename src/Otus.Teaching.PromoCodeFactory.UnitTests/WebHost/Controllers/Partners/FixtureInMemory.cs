using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.DataAccess;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;

namespace Otus.Teaching.PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class FixtureInMemory : IDisposable
    {
        public FixtureInMemory()
        {
            var options = new DbContextOptionsBuilder<PromoCodeContext>()
                .UseInMemoryDatabase(databaseName: "PromoCodeTest")
                .UseLazyLoadingProxies()
                .Options;

            Context = new PromoCodeContext(options);

            Seed();
        }

        public PromoCodeContext Context { get; private set; }

        private void Seed()
        {
            new DbInitializer(Context)
                .Initialize();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
