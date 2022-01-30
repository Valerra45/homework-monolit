using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class SetPartnerPromoCodeLimitRequestBuilder
    {
        private DateTime _createdEndDate;
        private int _createdLimit;

        public SetPartnerPromoCodeLimitRequestBuilder()
        {

        }

        public SetPartnerPromoCodeLimitRequestBuilder WithCreatedEndDate(DateTime dt)
        {
            _createdEndDate = dt;

            return this;
        }

        public SetPartnerPromoCodeLimitRequestBuilder WithCreatedLimit(int limit)
        {
            _createdLimit = limit;

            return this;
        }

        public SetPartnerPromoCodeLimitRequest Build()
        {
            return new SetPartnerPromoCodeLimitRequest()
            {
                EndDate = _createdEndDate,
                Limit = _createdLimit
            };
        }
    }
}
