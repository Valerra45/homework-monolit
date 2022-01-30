using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Промокоды
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController
        : ControllerBase
    {
        private IRepository<PromoCode> _promoCodeRepository;
        private readonly IRepository<Preference> _preferenceRepository;
        private readonly IRepository<Customer> _customerRepository;

        public PromocodesController(IRepository<PromoCode> promoCodeRepository,
            IRepository<Preference> preferenceRepository,
            IRepository<Customer> customerRepository)
        {
            _promoCodeRepository = promoCodeRepository;
            _preferenceRepository = preferenceRepository;
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            var promoCode = await _promoCodeRepository.GetAllAsync();

            var response = promoCode.Select(x => new PromoCodeShortResponse()
            {
                Id = x.Id,
                Code = x.Code,
                BeginDate = x.BeginDate.ToString("yyyy-MM-dd"),
                EndDate = x.EndDate.ToString("yyyy-MM-dd"),
                PartnerName = x.PartnerName,
                ServiceInfo = x.ServiceInfo
            }).ToList();

            return Ok(response);
        }

        /// <summary>
        /// Получить промокод по id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodeAsync(Guid id)
        {
            var promoCode = await _promoCodeRepository.GetByIdAsync(id);

            var response = new PromoCodeShortResponse()
            {
                Id = promoCode.Id,
                Code = promoCode.Code,
                BeginDate = promoCode.BeginDate.ToString("yyyy-MM-dd"),
                EndDate = promoCode.EndDate.ToString("yyyy-MM-dd"),
                PartnerName = promoCode.PartnerName,
                ServiceInfo = promoCode.ServiceInfo
            };

            return Ok(response);
        }

        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            var preference = await _preferenceRepository
                .GetFirstWhere(x => x.Name.Equals(request.Preference));

            var promoCode = new PromoCode()
            {
                Id = Guid.NewGuid(),
                ServiceInfo = request.ServiceInfo,
                Code = request.PromoCode,
                PartnerName = request.PartnerName,
                Preference = preference,
                BeginDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(14)
            };

            await _promoCodeRepository.AddAsync(promoCode);

            var customers = await _customerRepository
                .GetWhere(x => x.Preferences.Contains(preference));

            foreach (var c in customers)
            {
                c.PromoCodes.Add(promoCode);
            }

            await _customerRepository.UpdateRangeAsync(customers);

            return CreatedAtAction(nameof(GetPromocodeAsync), new { id = promoCode.Id }, promoCode.Id);
        }
    }
}