using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController
        : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Role> _roleRepository;

        public EmployeesController(IRepository<Employee> employeeRepository,
            IRepository<Role> roleRepository)
        {
            _employeeRepository = employeeRepository;
            _roleRepository = roleRepository;
        }

        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return employeesModelList;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Role = new RoleItemResponse()
                {
                    Name = employee.Role.Name,
                    Description = employee.Role.Description
                },
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }

        [HttpPost]
        public async Task<ActionResult> AddEmployeeAsync(CreateOrEditEmployeeRequest employeeViewModel)
        {
            var roles = await _roleRepository.GetAllAsync();

            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = employeeViewModel.FirstName,
                LastName = employeeViewModel.LastName,
                Email = employeeViewModel.Email,
                AppliedPromocodesCount = employeeViewModel.AppliedPromocodesCount,
                Role = roles.FirstOrDefault(x => x.Name == employeeViewModel.Roles)
            };

            await _employeeRepository.AddAsync(employee);

            return CreatedAtAction(nameof(GetEmployeeByIdAsync), new { id = employee.Id }, employee);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateEmployeeAsync(Guid id, CreateOrEditEmployeeRequest employeeViewModel)
        {
            var roles = await _roleRepository.GetAllAsync();

            var employee = new Employee
            {
                Id = id,
                FirstName = employeeViewModel.FirstName,
                LastName = employeeViewModel.LastName,
                Email = employeeViewModel.Email,
                AppliedPromocodesCount = employeeViewModel.AppliedPromocodesCount,
                Role = roles.FirstOrDefault(x => x.Name == employeeViewModel.Roles)
            };

            await _employeeRepository.UpdateAsync(employee);

            return CreatedAtAction(nameof(GetEmployeeByIdAsync), new { id = employee.Id }, employee);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteEmployeeAsync(Guid id)
        {
            var entity = await _employeeRepository.GetByIdAsync(id);

            await _employeeRepository.DeleteAsync(entity);

            return NoContent();
        }
    }
}