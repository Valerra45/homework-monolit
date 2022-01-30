using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Предпочтения
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferenceController
        : ControllerBase
    {
        private readonly IRepository<Preference> _preferenceRepository;

        public PreferenceController(IRepository<Preference> preferenceRepository)
        {
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получить все предпочтения
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PrefernceResponse>> GetPreferencesAsync()
        {
            var preferences = await _preferenceRepository.GetAllAsync();

            var responce = preferences.Select(x => new PrefernceResponse()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return Ok(responce);
        }
    }
}
