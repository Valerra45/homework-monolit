using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class PartnerBuilder
    {
        private Guid _createdId;
        private string _createdName;
        private int _createdNumberIssuedPromoCodes;
        private bool _createdIsActive;
        private ICollection<PartnerPromoCodeLimit> _createdPartnerLimits;

        public PartnerBuilder()
        {

        }

        public PartnerBuilder WithCreatedId(Guid id)
        {
            _createdId = id;

            return this;
        }

        public PartnerBuilder WithCreatedName(string name)
        {
            _createdName = name;
            return this;
        }

        public PartnerBuilder WithCreatedNumberIssuedPromoCodes(int number)
        {
            _createdNumberIssuedPromoCodes = number;
            return this;
        }

        public PartnerBuilder WithCreatedIsActive(bool isActive)
        {
            _createdIsActive = isActive;
            return this;
        }

        public PartnerBuilder WithCreatedPartnerLimits(ICollection<PartnerPromoCodeLimit> partnerPromoCodeLimits)
        {
            _createdPartnerLimits = partnerPromoCodeLimits;
            return this;
        }

        public Partner Build()
        {
            return new Partner()
            {
                Id = _createdId,
                Name = _createdName,
                NumberIssuedPromoCodes = _createdNumberIssuedPromoCodes,
                IsActive = _createdIsActive,
                PartnerLimits = _createdPartnerLimits
            };
        }
    }
}
