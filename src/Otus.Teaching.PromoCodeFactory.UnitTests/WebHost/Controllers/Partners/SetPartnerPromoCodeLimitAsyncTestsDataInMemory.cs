using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess;
using Otus.Teaching.PromoCodeFactory.DataAccess.Repositories;
using Otus.Teaching.PromoCodeFactory.WebHost.Controllers;
using Xunit;

namespace Otus.Teaching.PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class SetPartnerPromoCodeLimitAsyncTestsDataInMemory : IClassFixture<FixtureInMemory>
    {
        private readonly PromoCodeContext _context;

        public SetPartnerPromoCodeLimitAsyncTestsDataInMemory(FixtureInMemory fixtureInMemory)
        {
            _context = fixtureInMemory.Context;
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_Saved_New_Limit_To_Database()
        {
            // Arrange
            var partnerId = Guid.Parse("7d994823-8226-4273-b063-1a95f3cc1df8");

            var request = new SetPartnerPromoCodeLimitRequestBuilder()
             .WithCreatedEndDate(DateTime.Now)
             .WithCreatedLimit(999)
             .Build();

            var partnerRepository = new EfRepository<Partner>(_context);
            var controller = new PartnersController(partnerRepository);

            // Act
            var result = await controller.SetPartnerPromoCodeLimitAsync(partnerId, request);
            var limitId = Guid.Parse((result as CreatedAtActionResult).RouteValues["limitId"].ToString());

            // Assert
            limitId.Should().NotBeEmpty();
        }
    }
}
