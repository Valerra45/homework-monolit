using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class PartnerPromoCodeLimitBuilder
    {
        private Guid _createdId;
        private Guid _createdPartnerId;
        private Partner _createdPartner;
        private DateTime _createdCreateDate;
        private DateTime _createdCancelDate;
        private DateTime _createdEndDate;
        private int _createdLimit;

        public PartnerPromoCodeLimitBuilder()
        {

        }

        public PartnerPromoCodeLimitBuilder WithCreatedId(Guid id)
        {
            _createdId = id;

            return this;
        }

        public PartnerPromoCodeLimitBuilder WithCreatedPartnerId(Guid id)
        {
            _createdPartnerId = id;

            return this;
        }

        public PartnerPromoCodeLimitBuilder WithCreatedPartner(Partner partner)
        {
            _createdPartner = partner;

            return this;
        }

        public PartnerPromoCodeLimitBuilder WithCreatedCreateDate(DateTime dt)
        {
            _createdCreateDate = dt;

            return this;
        }

        public PartnerPromoCodeLimitBuilder WithCreatedCancelDate(DateTime dt)
        {
            _createdCancelDate = dt;

            return this;
        }

        public PartnerPromoCodeLimitBuilder WithCreatedEndDate(DateTime dt)
        {
            _createdEndDate = dt;

            return this;
        }

        public PartnerPromoCodeLimitBuilder WithCreatedLimit(int limit)
        {
            _createdLimit = limit;

            return this;
        }

        public PartnerPromoCodeLimit Build()
        {
            return new PartnerPromoCodeLimit()
            {
                Id = _createdId,
                PartnerId = _createdPartnerId,
                Partner = _createdPartner,
                Limit = _createdLimit,
                CreateDate = _createdCreateDate,
                EndDate = _createdEndDate
            };
        }
    }
}
