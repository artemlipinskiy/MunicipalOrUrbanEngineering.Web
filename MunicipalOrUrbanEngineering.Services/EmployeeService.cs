using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MunicipalOrUrbanEngineering.DataTransferObjects.Account;
using MunicipalOrUrbanEngineering.Entities;

namespace MunicipalOrUrbanEngineering.Services
{
    public class EmployeeService
    {
        private readonly MUEContext db;

        public EmployeeService(MUEContext db)
        {
            this.db = db;
        }

        private async Task<Employee> GetEntity(Guid Id)
        {
            try
            {
                return await db.Employees.FirstOrDefaultAsync(x => x.Id == Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<EmployeeViewDto> GetEmployeeInfo(Guid UserId)
        {
            try
            {
                var employee = await db.Employees.FirstOrDefaultAsync(x => x.AppUserId == UserId);
                return new EmployeeViewDto
                {
                    AppUserId = employee.AppUserId,
                    FirstName = employee.FirstName,
                    Id = employee.Id,
                    LastName = employee.LastName,
                    MiddleName = employee.MiddleName,
                    Fullname = employee.LastName + " "+ employee.FirstName + " " + employee.MiddleName
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<bool> Create(CreateEmployeeDto dto)
        {
            try
            {
                var employee = new Employee
                {
                    Id = Guid.NewGuid(),
                    AppUserId = dto.AppUserId,
                    FirstName = dto.FirstName,
                    MiddleName = dto.MiddleName,
                    LastName = dto.LastName
                };
                await db.Employees.AddAsync(employee);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<EmployeeViewDto> Get(Guid Id)
        {
            try
            {
                var entity = await GetEntity(Id);
                
                return new EmployeeViewDto
                {
                    AppUserId = entity.AppUserId,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Id = entity.Id,
                    MiddleName = entity.MiddleName,
                    Fullname = entity.LastName + " " + entity.FirstName + " " + entity.MiddleName
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IList<EmployeeViewDto>> List()
        {
            try
            {
                return await db.Employees.Select(emp => new EmployeeViewDto
                {
                    Id = emp.Id,
                    AppUserId = emp.AppUserId,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    MiddleName = emp.MiddleName,
                    Fullname = emp.LastName + " " + emp.FirstName + " " + emp.MiddleName
                }).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
