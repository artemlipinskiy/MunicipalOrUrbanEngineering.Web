using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MunicipalOrUrbanEngineering.DataTransferObjects;
using MunicipalOrUrbanEngineering.DataTransferObjects.Payment;
using MunicipalOrUrbanEngineering.Entities;

namespace MunicipalOrUrbanEngineering.Services
{
    public class ServiceTypeService
    {
        private readonly MUEContext db;
        private readonly HistoryService historyService;
        public ServiceTypeService(MUEContext db)
        {
            this.db = db;
            historyService = new HistoryService(db);
        }

        public async Task<IList<ViewServiceTypeDto>> List(bool IsCounterReadings)
        {
            try
            {
                return await db.ServiceTypes
                    .Where(x => x.IsCounterReadings == IsCounterReadings)
                    .Select(entity => new ViewServiceTypeDto
                    {
                        Id = entity.Id,
                        CreationDate = entity.CreationDate,
                        ModifyDate = entity.ModifyDate,
                        Description = entity.Description,
                        IsCounterReadings = entity.IsCounterReadings ? "Да" : "Нет",
                        Name = entity.Name,
                        UnitName = entity.UnitName
                    }).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IList<ViewServiceTypeDto>> List()
        {
            try
            {
                return await db.ServiceTypes.Select(entity => new ViewServiceTypeDto
                {
                    Id = entity.Id,
                    CreationDate = entity.CreationDate,
                    ModifyDate = entity.ModifyDate,
                    Description = entity.Description,
                    IsCounterReadings = entity.IsCounterReadings ? "Да" : "Нет",
                    Name = entity.Name,
                    UnitName = entity.UnitName
                }).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<ViewServiceTypeDto> Get(Guid id)
        {
            try
            {
                var entity = await GetEntity(id);
                return new ViewServiceTypeDto
                {
                    Id = entity.Id,
                    CreationDate = entity.CreationDate,
                    ModifyDate = entity.ModifyDate,
                    Description = entity.Description,
                    IsCounterReadings = entity.IsCounterReadings ? "Да" : "Нет",
                    Name = entity.Name,
                    UnitName = entity.UnitName
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

      
        private string GetDetailsUpdate(ServiceType fromdb, ViewServiceTypeDto dto)
        {
            try
            {
                string Details = string.Empty;
                if (fromdb.Name != dto.Name)
                {
                    Details += $"Property ({nameof(ServiceType.Name)}): From {fromdb.Name} to {dto.Name} \n";
                }

                if (fromdb.Description != dto.Description)
                {
                    Details += $"Property ({nameof(ServiceType.Description)}): From {fromdb.Description} to {dto.Description} \n";
                }

                if (fromdb.UnitName != dto.UnitName)
                {
                    Details += $"Property ({nameof(ServiceType.UnitName)}): From {fromdb.UnitName} to {dto.UnitName} \n";
                }


                return Details;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<bool> Create(CreateServiceTypeDto dto, Guid? employeeId)
        {
            try
            {
                Guid Id = Guid.NewGuid();
                var entity = new ServiceType
                {
                    Id = Id,
                    CreationDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    Description = dto.Description,
                    IsCounterReadings = dto.IsCounterReadings,
                    Name = dto.Name,
                    UnitName = dto.UnitName
                };

                await db.ServiceTypes.AddAsync(entity);
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.Create,
                    CreationDate = DateTime.Now,
                    Details = null,
                    EmployeeId = employeeId,
                    Entity = nameof(ServiceType),
                    EntityId = Id,
                    OwnerId = null
                    
                });
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<bool> Delete(Guid Id, Guid? EmployeeId)
        {
            try
            {

                var entity = await GetEntity(Id);
                db.ServiceTypes.Remove(entity);
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.Delete,
                    CreationDate = DateTime.Now,
                    Details = null,
                    EmployeeId = EmployeeId,
                    Entity = nameof(ServiceType),
                    EntityId = Id,
                    OwnerId = null
                });
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<ServiceType> GetEntity(Guid Id)
        {
            return await db.ServiceTypes.FirstOrDefaultAsync(x => x.Id == Id);
        }
    }
}
