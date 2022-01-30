using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Controllers;
using Xunit;

namespace Otus.Teaching.PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class SetPartnerPromoCodeLimitAsyncTests
    {
        private readonly Mock<IRepository<Partner>> _partnersRepositoryMock;
        private readonly PartnersController _partnersController;

        public SetPartnerPromoCodeLimitAsyncTests()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            _partnersRepositoryMock = fixture.Freeze<Mock<IRepository<Partner>>>();

            _partnersController = fixture.Build<PartnersController>().OmitAutoProperties().Create();
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerIsNotFound_ReturnsNotFound()
        {
            // Arrange
            var partnerId = Guid.Parse("def47943-7aaf-44a1-ae21-05aa4948b165");
            Partner partner = null;

            var request = new SetPartnerPromoCodeLimitRequestBuilder()
             .WithCreatedEndDate(DateTime.Now)
             .WithCreatedLimit(100)
             .Build();

            _partnersRepositoryMock.Setup(x => x.GetByIdAsync(partnerId)).ReturnsAsync(partner);

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            result.Should().BeAssignableTo<NotFoundResult>();
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_PartnerIsNotActive_ReturnsBadRequest()
        {
            // Arrange
            var partnerId = Guid.Parse("def47943-7aaf-44a1-ae21-05aa4948b165");

            var request = new SetPartnerPromoCodeLimitRequestBuilder()
                .WithCreatedEndDate(DateTime.Now)
                .WithCreatedLimit(100)
                .Build();

            var partner = new PartnerBuilder()
                .WithCreatedId(Guid.NewGuid())
                .WithCreatedIsActive(false)
                .WithCreatedPartnerLimits(new List<PartnerPromoCodeLimit>() { new PartnerPromoCodeLimit() })
                .Build();

            _partnersRepositoryMock.Setup(x => x.GetByIdAsync(partnerId)).ReturnsAsync(partner);

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
               .Which.Value.Should().Be("Данный партнер не активен");
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_If_Partner_Given_Limit_Reset_NumberIssuedPromoCodes()
        {
            // Arrange
            var partnerId = Guid.Parse("def47943-7aaf-44a1-ae21-05aa4948b165");

            var request = new SetPartnerPromoCodeLimitRequestBuilder()
                .WithCreatedEndDate(DateTime.Now)
                .WithCreatedLimit(1)
                .Build();

            var partner = new PartnerBuilder()
                .WithCreatedId(Guid.NewGuid())
                .WithCreatedIsActive(true)
                .WithCreatedNumberIssuedPromoCodes(444)
                .Build();

            var partnerPromoCodeLimit = new PartnerPromoCodeLimitBuilder()
                .WithCreatedId(Guid.NewGuid())
                .WithCreatedPartnerId(partner.Id)
                .WithCreatedPartner(partner)
                .WithCreatedCreateDate(DateTime.Now)
                .WithCreatedEndDate(DateTime.Now.AddDays(3))
                .WithCreatedLimit(100)
                .Build();

            partner.PartnerLimits = new List<PartnerPromoCodeLimit>() { partnerPromoCodeLimit };

            _partnersRepositoryMock.Setup(x => x.GetByIdAsync(partnerId)).ReturnsAsync(partner);

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            partner.NumberIssuedPromoCodes.Should().Be(0);
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_When_Setting_Limit_Disable_Previous_Limit()
        {
            // Arrange
            var partnerId = Guid.Parse("def47943-7aaf-44a1-ae21-05aa4948b165");

            var request = new SetPartnerPromoCodeLimitRequestBuilder()
                .WithCreatedEndDate(DateTime.Now)
                .WithCreatedLimit(1)
                .Build();

            var partner = new PartnerBuilder()
                .WithCreatedId(Guid.NewGuid())
                .WithCreatedIsActive(true)
                .WithCreatedNumberIssuedPromoCodes(444)
                .Build();

            var partnerPromoCodeLimit = new PartnerPromoCodeLimitBuilder()
                .WithCreatedId(Guid.NewGuid())
                .WithCreatedPartnerId(partner.Id)
                .WithCreatedPartner(partner)
                .WithCreatedCreateDate(DateTime.Now)
                .WithCreatedEndDate(DateTime.Now.AddDays(3))
                .WithCreatedLimit(100)
                .Build();

            partner.PartnerLimits = new List<PartnerPromoCodeLimit>() { partnerPromoCodeLimit };

            _partnersRepositoryMock.Setup(x => x.GetByIdAsync(partnerId)).ReturnsAsync(partner);

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            partnerPromoCodeLimit.Should().NotBeNull();
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_The_Limit_Must_Be_Greater_Than_Zero()
        {
            // Arrange
            var partnerId = Guid.Parse("def47943-7aaf-44a1-ae21-05aa4948b165");

            var request = new SetPartnerPromoCodeLimitRequestBuilder()
                .WithCreatedEndDate(DateTime.Now)
                .WithCreatedLimit(0)
                .Build();

            var partner = new PartnerBuilder()
                .WithCreatedId(Guid.NewGuid())
                .WithCreatedIsActive(true)
                .WithCreatedPartnerLimits(new List<PartnerPromoCodeLimit>() { new PartnerPromoCodeLimit() })
                .Build();

            _partnersRepositoryMock.Setup(x => x.GetByIdAsync(partnerId)).ReturnsAsync(partner);

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
             .Which.Value.Should().Be("Лимит должен быть больше 0");
        }
    }
}