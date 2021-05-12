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
    public class TariffSerivce
    {
        private readonly MUEContext db;
        private readonly HistoryService historyService;
        private readonly ServiceTypeService serviceTypeService;
        private readonly BuildingService buildingService;
        public TariffSerivce(MUEContext db)
        {
            this.db = db;
            historyService = new HistoryService(db);
            serviceTypeService = new ServiceTypeService(db);
            buildingService = new BuildingService(db);
        }

        private async Task<Tariff> GetEntity(Guid Id)
        {
            return await db.Tariffs.FirstOrDefaultAsync(x => x.Id == Id);
        }

        private string GetUpdateDetails(Tariff tariffdb, UpdateTariffDto dto)
        {
            try
            {
                string Details = null;
                if (tariffdb.Value != dto.Value)
                {
                    Details += $"Property ({nameof(dto.Value)}) from {tariffdb.Value} to {dto.Value} \n";
                }

                if (tariffdb.DefaultData != dto.DefaultData)
                {
                    Details += $"Property ({nameof(dto.DefaultData)}) from {tariffdb.DefaultData} to {dto.DefaultData} \n";
                }
                string dateBefore = tariffdb.EndTariff.HasValue ? $"{tariffdb.EndTariff.Value.Day}.{tariffdb.EndTariff.Value.Month}.{tariffdb.EndTariff.Value.Year}" : "null";
                string dateAfter = tariffdb.EndTariff.HasValue ? $"{dto.EndTariff.Value.Day}.{dto.EndTariff.Value.Month}.{dto.EndTariff.Value.Year}" : "null";
                if (dateBefore != dateAfter)
                {
                    Details += $"Property ({nameof(dto.EndTariff)}) from {dateBefore} to {dateAfter} \n";
                }
                return Details;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Guid> UpdateValue(UpdateTariffDto dto, Guid? employeeId)
        {
            try
            {
                var entity = await GetEntity(dto.Id);
                //TODO: получить последний счет по этому тарифу, чтобы узнать по периоду, какая последняя дата тарифа
                Guid Id = Guid.NewGuid();
                var NewEntity = new Tariff
                {
                    Id = Id,
                    CreationDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    StartTariff = dto.StartTariff,
                    DefaultData = dto.DefaultData,
                    Value = dto.Value,
                    EndTariff = null,
                    FlatId = entity.FlatId,
                    ServiceTypeId = entity.ServiceTypeId
                };
                await db.AddAsync(NewEntity);
                await db.SaveChangesAsync();
                
                return Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        } 
        public async Task<bool> Update(UpdateTariffDto dto, Guid? employeeId)
        {
            try
            {
                //TODO: проверять есть ли счета с этим тарифом
                var entity = await GetEntity(dto.Id);
                var Details = GetUpdateDetails(entity, dto);
                entity.EndTariff = dto.EndTariff;
                entity.Value = dto.Value;
                entity.DefaultData = dto.DefaultData;

                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.Update,
                    CreationDate = DateTime.Now,
                    Details = Details,
                    EmployeeId = employeeId,
                    Entity = nameof(Tariff),
                    EntityId = entity.Id,
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
                //TODO: проверять есть ли счета с этим тарифом
                var entity = await GetEntity(Id);
                db.Tariffs.Remove(entity);
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.Delete,
                    CreationDate = DateTime.Now,
                    Details = null,
                    EmployeeId = EmployeeId,
                    Entity = nameof(Tariff),
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
        public async Task<bool> End(Guid Id, Guid? EmployeeId)
        {
            try
            {
                var entity = await GetEntity(Id);
               
                entity.EndTariff  = GetEndMonth(DateTime.Now);

                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.Update,
                    CreationDate = DateTime.Now,
                    Details = $"Set EndDate to {entity.EndTariff}",
                    EmployeeId = EmployeeId,
                    Entity = nameof(Tariff),
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
        public async Task<Guid?> Create(CreateTariffDto dto, Guid? EmployeeId)
        {
            try
            {
                dto.StartTariff = GetStartMonth(dto.StartTariff);
                if (dto.EndTariff.HasValue)
                {
                    dto.EndTariff = GetEndMonth(dto.EndTariff.Value);
                }
                
                var tariffs = await List(dto.FlatId, dto.ServiceTypeId);
                if (tariffs.Count > 0)
                {
                    foreach (var viewTariffDto in tariffs)
                    {
                        viewTariffDto.EndTariff ??= DateTime.MaxValue;
                    }

                    var startDate = dto.StartTariff;
                    var endDate = dto.EndTariff.HasValue ? dto.EndTariff.Value : DateTime.MaxValue;
                    var count = tariffs.Count(x => !((x.StartTariff < startDate && x.EndTariff < endDate && x.EndTariff < endDate &&
                                                    x.EndTariff < startDate)
                                                   || (x.StartTariff > startDate && x.EndTariff > endDate && x.EndTariff > endDate &&
                                                       x.EndTariff > startDate)));
                    if (count > 0)
                    {
                        return null;
                    }

                }
                Guid Id = Guid.NewGuid();
                var entity = new Tariff
                {
                    Id = Id,
                    CreationDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    StartTariff = dto.StartTariff,
                    DefaultData = dto.DefaultData,
                    EndTariff = dto.EndTariff,
                    FlatId = dto.FlatId,
                    ServiceTypeId = dto.ServiceTypeId,
                    Value = dto.Value
                };
                await db.Tariffs.AddAsync(entity);
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.Create,
                    CreationDate = DateTime.Now,
                    Details = null,
                    EmployeeId = EmployeeId,
                    Entity = nameof(Tariff),
                    EntityId = Id,
                    OwnerId = null
                });
                
                return Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ViewTariffDto> Get(Guid id)
        {
            try
            {
                var entity = await GetEntity(id);
                var flat = await buildingService.GetFlat(entity.FlatId);
                var serviceType = await serviceTypeService.Get(entity.ServiceTypeId);

                return new ViewTariffDto
                {
                    Id = entity.Id,
                    CreationDate = entity.CreationDate,
                    ModifyDate = entity.ModifyDate,
                    StartTariff = entity.StartTariff,
                    EndTariff = entity.EndTariff,
                    Flat = flat.FullAddress,
                    FlatId = entity.FlatId,
                    ServiceType = $"{serviceType.Name} Счетчик({serviceType.IsCounterReadings})",
                    ServiceTypeId = entity.Id,
                    Value = entity.Value,
                    DefaultData = entity.DefaultData,
                    UnitName = serviceType.UnitName
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IList<ViewTariffDto>> List(Guid flatId)
        {
            try
            {
               var tariffs = await db.Tariffs
                    .Where(x => x.FlatId == flatId)
                    .Include(x => x.Flat)
                        .ThenInclude(x => x.Building)
                        .ThenInclude(x => x.Street)
                    .Include(x => x.ServiceType)
                    .ToListAsync();
               return tariffs.Select(x => new ViewTariffDto
               {
                   Id = x.Id,
                   CreationDate = x.CreationDate,
                   ModifyDate = x.ModifyDate,
                   StartTariff = x.StartTariff,
                   EndTariff = x.EndTariff,
                   FlatId = x.FlatId,
                   ServiceTypeId = x.ServiceTypeId,
                   Flat = $"{Constants.BaseWord.Street} {x.Flat.Building.Street.Name} {Constants.BaseWord.Building} {x.Flat.Building.Name} {Constants.BaseWord.Apartament} {x.Flat.ApartmentNumber}",
                   ServiceType = x.ServiceType.IsCounterReadings ? $"{x.ServiceType.Name} Счетчик(Да)" : $"{x.ServiceType.Name} Счетчик(Нет)",
                   UnitName = x.ServiceType.UnitName,
                   Value = x.Value,
                   DefaultData = x.DefaultData
               }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IList<ViewTariffDto>> List(Guid flatId, DateTime date)
        {
            try
            {
                var tariffs = await db.Tariffs
                    .Where(x => x.FlatId == flatId)
                    .Where(x => x.StartTariff <= date && (x.EndTariff == null || date <= x.EndTariff.Value))
                    .Include(x => x.Flat)
                    .ThenInclude(x => x.Building)
                    .ThenInclude(x => x.Street)
                    .Include(x => x.ServiceType)
                    .ToListAsync();
                 return tariffs.Select(x => new ViewTariffDto
                {
                    Id = x.Id,
                    CreationDate = x.CreationDate,
                    ModifyDate = x.ModifyDate,
                    StartTariff = x.StartTariff,
                    EndTariff = x.EndTariff,
                    FlatId = x.FlatId,
                    ServiceTypeId = x.ServiceTypeId,
                    Flat = $"{Constants.BaseWord.Street} {x.Flat.Building.Street.Name} {Constants.BaseWord.Building} {x.Flat.Building.Name} {Constants.BaseWord.Apartament} {x.Flat.ApartmentNumber}",
                    ServiceType = x.ServiceType.IsCounterReadings ? $"{x.ServiceType.Name} Счетчик(Да)" : $"{x.ServiceType.Name} Счетчик(Нет)",
                    UnitName = x.ServiceType.UnitName,
                    Value = x.Value,
                    DefaultData = x.DefaultData
                }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<IList<ViewTariffDto>> List(Guid flatId, Guid servicetypeId)
        {
            try
            {
                var tariffs = await db.Tariffs
                    .Where(x => x.FlatId == flatId)
                    .Where(x => servicetypeId == x.ServiceTypeId)
                    .Include(x => x.Flat)
                    .ThenInclude(x => x.Building)
                    .ThenInclude(x => x.Street)
                    .Include(x => x.ServiceType)
                    .ToListAsync();
                return tariffs.Select(x => new ViewTariffDto
                {
                    Id = x.Id,
                    CreationDate = x.CreationDate,
                    ModifyDate = x.ModifyDate,
                    StartTariff = x.StartTariff,
                    EndTariff = x.EndTariff,
                    FlatId = x.FlatId,
                    ServiceTypeId = x.ServiceTypeId,
                    Flat = $"{Constants.BaseWord.Street} {x.Flat.Building.Street.Name} {Constants.BaseWord.Building} {x.Flat.Building.Name} {Constants.BaseWord.Apartament} {x.Flat.ApartmentNumber}",
                    ServiceType = x.ServiceType.IsCounterReadings ? $"{x.ServiceType.Name} Счетчик(Да)" : $"{x.ServiceType.Name} Счетчик(Нет)",
                    UnitName = x.ServiceType.UnitName,
                    Value = x.Value,
                    DefaultData = x.DefaultData
                }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IList<ViewTariffDto>> List(DateTime date)
        {
            try
            {
                var tariffs = await db.Tariffs
                    .Where(x => x.StartTariff.Date <= date.Date && (x.EndTariff == null || x.EndTariff.Value.Date >= date.Date))
                    .Include(x => x.Flat)
                    .ThenInclude(x => x.Building)
                    .ThenInclude(x => x.Street)
                    .Include(x => x.ServiceType)
                    .ToListAsync();
                return tariffs.Select(x => new ViewTariffDto
                {
                    Id = x.Id,
                    CreationDate = x.CreationDate,
                    ModifyDate = x.ModifyDate,
                    StartTariff = x.StartTariff,
                    EndTariff = x.EndTariff,
                    FlatId = x.FlatId,
                    ServiceTypeId = x.ServiceTypeId,
                    Flat = $"{Constants.BaseWord.Street} {x.Flat.Building.Street.Name} {Constants.BaseWord.Building} {x.Flat.Building.Name} {Constants.BaseWord.Apartament} {x.Flat.ApartmentNumber}",
                    ServiceType = x.ServiceType.IsCounterReadings ? $"{x.ServiceType.Name} Счетчик(Да)" : $"{x.ServiceType.Name} Счетчик(Нет)",
                    UnitName = x.ServiceType.UnitName,
                    Value = x.Value,
                    DefaultData = x.DefaultData
                }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IList<ViewTariffDto>> List(DateTime date, bool withcounter = false)
        {
            try
            {
                var tariffs = await db.Tariffs
                    .Where(x => x.StartTariff.Date <= date.Date && (x.EndTariff == null || x.EndTariff.Value.Date >= date.Date))
                    .Include(x => x.Flat)
                    .ThenInclude(x => x.Building)
                    .ThenInclude(x => x.Street)
                    .Include(x => x.ServiceType)
                    .Where(x => x.ServiceType.IsCounterReadings == withcounter)
                    .ToListAsync();
                return tariffs.Select(x => new ViewTariffDto
                {
                    Id = x.Id,
                    CreationDate = x.CreationDate,
                    ModifyDate = x.ModifyDate,
                    StartTariff = x.StartTariff,
                    EndTariff = x.EndTariff,
                    FlatId = x.FlatId,
                    ServiceTypeId = x.ServiceTypeId,
                    Flat = $"{Constants.BaseWord.Street} {x.Flat.Building.Street.Name} {Constants.BaseWord.Building} {x.Flat.Building.Name} {Constants.BaseWord.Apartament} {x.Flat.ApartmentNumber}",
                    ServiceType = x.ServiceType.IsCounterReadings ? $"{x.ServiceType.Name} Счетчик(Да)" : $"{x.ServiceType.Name} Счетчик(Нет)",
                    UnitName = x.ServiceType.UnitName,
                    Value = x.Value,
                    DefaultData = x.DefaultData
                }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IList<ViewTariffDto>> ListTariffWithCounter(Guid flatId, DateTime start, DateTime end)
        {
            try
            {
                var tariffs = await db.Tariffs
                    .Where(x => x.FlatId == flatId)
                    .Include(x => x.Flat)
                    .ThenInclude(x => x.Building)
                    .ThenInclude(x => x.Street)
                    .Include(x => x.ServiceType)
                    .Where(x => x.ServiceType.IsCounterReadings)
                    .ToListAsync();
                foreach (var tariff in tariffs)
                {
                    tariff.EndTariff ??= DateTime.MaxValue;
                }
                tariffs = tariffs.Where(x => !(
                    (x.StartTariff > start && x.StartTariff > end && x.EndTariff > start && x.EndTariff > end)
                    || (x.StartTariff < start && x.StartTariff < end && x.EndTariff < start && x.EndTariff < end)
                )).ToList();
                foreach (var tariff in tariffs)
                {
                    tariff.EndTariff = tariff.EndTariff == DateTime.MaxValue ? null : tariff.EndTariff;
                }
                return tariffs.Select(x => new ViewTariffDto
                {
                    Id = x.Id,
                    CreationDate = x.CreationDate,
                    ModifyDate = x.ModifyDate,
                    StartTariff = x.StartTariff,
                    EndTariff = x.EndTariff,
                    FlatId = x.FlatId,
                    ServiceTypeId = x.ServiceTypeId,
                    Flat = $"{Constants.BaseWord.Street} {x.Flat.Building.Street.Name} {Constants.BaseWord.Building} {x.Flat.Building.Name} {Constants.BaseWord.Apartament} {x.Flat.ApartmentNumber}",
                    ServiceType = x.ServiceType.IsCounterReadings ? $"{x.ServiceType.Name} Счетчик(Да)" : $"{x.ServiceType.Name} Счетчик(Нет)",
                    UnitName = x.ServiceType.UnitName,
                    Value = x.Value,
                    DefaultData = x.DefaultData
                }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        //TODO
        private static DateTime GetStartMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }
        private static DateTime GetEndMonth(DateTime date)
        {
            date = date.AddMonths(1);
            return GetStartMonth(date).AddDays(-1);
        }
    }
}
