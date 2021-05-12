using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MunicipalOrUrbanEngineering.DataTransferObjects;
using MunicipalOrUrbanEngineering.DataTransferObjects.Account;
using MunicipalOrUrbanEngineering.Entities;

namespace MunicipalOrUrbanEngineering.Services
{
    public class OwnerService
    {
        private readonly MUEContext db;
        private readonly HistoryService historyService;
        public OwnerService(MUEContext db)
        {
            this.db = db;
            this.historyService = new HistoryService(db);
        }

        private async Task<Owner> GetEntity(Guid Id)
        {
            try
            {
                return await db.Owners.FirstOrDefaultAsync(x => x.Id == Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> Create(CreateOwnerDto dto, Guid? EmployeeId)
        {
            try
            {
                var Id = Guid.NewGuid();
                var owner = new Owner
                {
                    Id = Id,
                    AppUserId = dto.AppUserId,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    MiddleName = dto.MiddleName
                };
                await db.AddAsync(owner);
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.Create,
                    CreationDate = DateTime.Now,
                    Details = null,
                    EmployeeId = EmployeeId,
                    Entity = nameof(Owner),
                    EntityId = Id
                });
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ViewOwnerDto> GetOwnerInfo(Guid UserId)
        {
            try
            {
                var employee = await db.Owners.Include(x => x.Flats).FirstOrDefaultAsync(x => x.AppUserId == UserId);
                return new ViewOwnerDto
                {
                    AppUserId = employee.AppUserId,
                    FirstName = employee.FirstName,
                    Id = employee.Id,
                    LastName = employee.LastName,
                    MiddleName = employee.MiddleName,
                    Fullname = employee.LastName + " " + employee.FirstName + " " + employee.MiddleName,
                    FlatIds = employee.Flats.Select(x => x.Id).ToList()
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<ViewOwnerDto> Get(Guid Id)
        {
            try
            {
                var owner = await db.Owners.Include(x => x.Flats).FirstOrDefaultAsync(x => x.Id == Id);
                return new ViewOwnerDto
                {
                    AppUserId = owner.AppUserId,
                    FirstName = owner.FirstName,
                    Id = owner.Id,
                    LastName = owner.LastName,
                    MiddleName = owner.MiddleName,
                    Fullname = owner.LastName + " " + owner.FirstName + " " + owner.MiddleName,
                    FlatIds = owner.Flats.Select(x => x.Id).ToList()
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IList<ViewOwnerDto>> List()
        {
            try
            {
                return await db.Owners.Include(x => x.Flats).Select(
                        owner => new ViewOwnerDto
                        {
                            Id = owner.Id,
                            AppUserId = owner.AppUserId,
                            FirstName = owner.FirstName,
                            LastName = owner.LastName,
                            MiddleName = owner.MiddleName,
                            Fullname = owner.LastName + " " + owner.FirstName + " " + owner.MiddleName,
                            FlatIds = owner.Flats.Select(x => x.Id).ToList()
                        })
                    .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<IList<ViewOwnerDto>> ListWithoutFlat()
        {
            try
            {
                return await db.Owners.Include(x => x.Flats)
                    .Where(x => x.Flats.Count == 0)
                    .Select(
                        owner => new ViewOwnerDto
                        {
                            Id = owner.Id,
                            AppUserId = owner.AppUserId,
                            FirstName = owner.FirstName,
                            LastName = owner.LastName,
                            MiddleName = owner.MiddleName,
                            Fullname = owner.LastName + " " + owner.FirstName + " " + owner.MiddleName
                        })
                    .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
