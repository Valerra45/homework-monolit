using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Preference> _preferenceRepository;

        public CustomersController(IRepository<Customer> customerRepository,
            IRepository<Preference> preferenceRepository)
        {
            _customerRepository = customerRepository;

            _preferenceRepository = preferenceRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerShortResponse>> GetCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync();

            var response = customers.Select(x => new CustomerShortResponse()
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName
            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            var response = new CustomerResponse()
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PrefernceResponses = customer.Preferences.Select(x => new PrefernceResponse()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList()
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var preferences = await _preferenceRepository.GetRangeByIdsAsync(request.PreferenceIds);

            var customer = new Customer()
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Preferences = (List<Preference>)preferences
            };

            await _customerRepository.AddAsync(customer);

            return CreatedAtAction(nameof(GetCustomerAsync), new { id = customer.Id }, customer.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            var preferences = await _preferenceRepository.GetRangeByIdsAsync(request.PreferenceIds);

            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Email = request.Email;
            customer.Preferences.Clear();
            customer.Preferences.AddRange(preferences);

            await _customerRepository.UpdateAsync(customer);

            return CreatedAtAction(nameof(GetCustomerAsync), new { id = customer.Id }, customer.Id);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            await _preferenceRepository.DeleteRangeAsync(customer.Preferences);

            await _customerRepository.DeleteAsync(customer);

            return NoContent();
        }
    }
}