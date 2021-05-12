using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MunicipalOrUrbanEngineering.DataTransferObjects;
using MunicipalOrUrbanEngineering.Entities;

namespace MunicipalOrUrbanEngineering.Services
{
    public class HistoryService
    {
        private readonly MUEContext db;

        public HistoryService(MUEContext db)
        {
            this.db = db;
        }

        private async Task<History> GetEntity(Guid Id)
        {
            try
            {
                return await db.Histories
                    .Include(x => x.Employee)
                    .Include(x => x.Owner)
                    .FirstOrDefaultAsync(x => x.Id == Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<HistoryView> Get(Guid Id)
        {
            try
            {
                var entity = await db.Histories
                    .Include(x => x.Employee)
                    .Include(x => x.Owner)
                    .FirstOrDefaultAsync(x => x.Id == Id);
                if (entity == null)
                {
                    return null;
                }

                return new HistoryView
                {
                    Id = entity.Id,
                    Action = entity.Action,
                    CreationDate = entity.CreationDate,
                    Details = entity.Details,
                    Employee = entity.EmployeeId == null? string.Empty : $"{ entity.Employee.LastName} {entity.Employee.FirstName} {entity.Employee.MiddleName}",
                    EmployeeId = entity.EmployeeId,
                    Entity = entity.Entity,
                    EntityId = entity.EntityId,
                    Owner = entity.OwnerId == null ? string.Empty : $"{ entity.Owner.LastName} {entity.Owner.FirstName} {entity.Owner.MiddleName}",
                    OwnerId = entity.OwnerId
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<IList<HistoryView>> All()
        {
            try
            {
                var entitiess = await db.Histories
                    .Include(x => x.Employee)
                    .Include(x => x.Owner).ToListAsync();
                if (entitiess == null)
                {
                    return null;
                }

                return entitiess.Select(entity => new HistoryView
                {
                    Id = entity.Id,
                    Action = entity.Action,
                    CreationDate = entity.CreationDate,
                    Details = entity.Details,
                    Employee = entity.EmployeeId == null ? string.Empty : $"{ entity.Employee.LastName} {entity.Employee.FirstName} {entity.Employee.MiddleName}",
                    EmployeeId = entity.EmployeeId,
                    Entity = entity.Entity,
                    EntityId = entity.EntityId,
                    Owner = entity.OwnerId == null ? string.Empty : $"{ entity.Owner.LastName} {entity.Owner.FirstName} {entity.Owner.MiddleName}",
                    OwnerId = entity.OwnerId
                }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<Page<HistoryView>> List(int pageSize = 10, int page = 1)
        {
            try
            {
                var entitiess = await db.Histories
                    .OrderByDescending(x => x.CreationDate)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                var total = await db.Histories.CountAsync();
                if (entitiess == null)
                {
                    return null;
                }

                var employees = await db.Employees.ToListAsync();
                var owners = await db.Owners.ToListAsync();

                var dto = entitiess.Select(entity => new HistoryView
                {
                    Id = entity.Id,
                    Action = entity.Action,
                    CreationDate = entity.CreationDate,
                    Details = entity.Details,
                    Employee = entity.EmployeeId == null ? string.Empty : GetFullName(entity.EmployeeId, entity.OwnerId, employees, owners),
                    EmployeeId = entity.EmployeeId,
                    Entity = entity.Entity,
                    EntityId = entity.EntityId,
                    Owner = entity.OwnerId == null ? string.Empty : GetFullName(entity.EmployeeId, entity.OwnerId, employees, owners),
                    OwnerId = entity.OwnerId
                }).ToList();

                return new Page<HistoryView>(dto,total, pageSize, page);
                
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<bool> Create(CreateHistoryDto dto)
        {
            try
            {
                var History = new History
                {
                    Id = Guid.NewGuid(),
                    Action = dto.Action,
                    CreationDate = dto.CreationDate,
                    Details = dto.Details,
                    EmployeeId = dto.EmployeeId,
                    EntityId = dto.EntityId,
                    OwnerId = dto.OwnerId,
                    Entity = dto.Entity
                };
                await db.AddAsync(History);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private string GetFullName(Guid? employeeId, Guid? ownerId, List<Employee> employees, List<Owner> owners)
        {
            if (employeeId.HasValue)
            {
                var employee = employees.FirstOrDefault(x => x.Id == employeeId);
                return employee == null
                    ? string.Empty
                    : $"{employee.LastName} {employee.FirstName} {employee.MiddleName}";
            }
            if (ownerId.HasValue)
            {
                var owner = owners.FirstOrDefault(x => x.Id == ownerId);
                return owner == null
                    ? string.Empty
                    : $"{owner.LastName} {owner.FirstName} {owner.MiddleName}";
            }

            return string.Empty;
        }
        
    }
}
