using EmployeeAPI.Data;
using EmployeeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class EmployeeController : Controller
    {
        private readonly EmployeeAPIDbContext dbContext;

        public EmployeeController(EmployeeAPIDbContext dbContext)
        {
                this.dbContext = dbContext; 
        }
        [HttpGet]  
        public async Task<IActionResult> GetEmployees()
        {
            return Ok(await dbContext.Employees.ToListAsync());
            
        }

        [HttpGet]
        [Route("id;Guid")]
        public async Task<IActionResult> GetEmployee([FromRoute] Guid id)
        {
            var employee = await dbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();

            }
            return Ok(employee);
        }


        [HttpPost]
        public async Task<IActionResult> AddEmployee(AddEmployeeRequest addEmployeeRequest)
        {
            var Employee = new Employee()
            {
                Id = Guid.NewGuid(),
                EmployeeCity = addEmployeeRequest.EmployeeCity,
                EmployeeId = addEmployeeRequest.EmployeeId,
                EmployeeName = addEmployeeRequest.EmployeeName,

            };
            await dbContext.Employees.AddAsync(Employee); 
            await dbContext.SaveChangesAsync();

            return Ok(Employee);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] Guid id, UpdateEmployeeRequest updateEmployeeRequest)
        {
            var employee = await dbContext.Employees.FindAsync(id);

            if (employee != null)
            {
                employee.EmployeeName = updateEmployeeRequest.EmployeeName;
                employee.EmployeeCity = updateEmployeeRequest.EmployeeCity;
                employee.EmployeeId = updateEmployeeRequest.EmployeeId;

                await dbContext.SaveChangesAsync();

                return Ok(employee);
            }
            return NotFound();

        }

        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
        {
            var employee = await dbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();

            }
            dbContext.Remove(employee);
            dbContext.SaveChanges();
            return Ok(employee);
        }
    }
}
