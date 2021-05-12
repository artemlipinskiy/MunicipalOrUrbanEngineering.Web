using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MunicipalOrUrbanEngineering.DataTransferObjects;
using MunicipalOrUrbanEngineering.DataTransferObjects.Payment;
using MunicipalOrUrbanEngineering.Entities;

namespace MunicipalOrUrbanEngineering.Services
{
    public class MetersDataService
    {
        private readonly MUEContext db;
        private readonly HistoryService historyService;
        private readonly BuildingService buildingService;

        public MetersDataService(MUEContext db)
        {
            this.db = db;
            historyService = new HistoryService(db);
            buildingService = new BuildingService(db);
        }

        private async Task<MetersData> GetEntity(Guid Id)
        {
            return await db.MetersDatas.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<ViewMetersDataDto> GetLastMeters(Guid servicetypeId, Guid flatId)
        {
            try
            {
                //TODO: проверить правильность работы 
                var entities = await db.MetersDatas
                    .Include(x => x.Tariff)
                        .ThenInclude(x => x.ServiceType)
                    .Include(x => x.PaymentPeriod)
                    .Where(x => x.Tariff.ServiceTypeId == servicetypeId && x.Tariff.FlatId == flatId)
                    .OrderByDescending(x => x.PaymentPeriod.StartDate)
                    .ToListAsync();
                var entity = entities.FirstOrDefault();
                
                var flat = await buildingService.GetFlat(entity.Tariff.FlatId);
               
                return new ViewMetersDataDto
                {
                    Id = entity.Id,
                    Address = flat.FullAddress,
                    CreationDate = entity.CreationDate,
                    ModifyDate = entity.ModifyDate,
                    PaymentPeriod = entity.PaymentPeriod.Name,
                    PaymentPeriodId = entity.PaymentPeriodId,
                    TariffId = entity.TariffId,
                    Value = entity.Value,
                    ServiceType = entity.Tariff.ServiceType.Name
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> Delete(Guid Id, Guid? OwnerId, Guid? EmployeeId)
        {
            try
            {
                var entity = await GetEntity(Id);
                db.MetersDatas.Remove(entity);
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.Delete,
                    CreationDate = DateTime.Now,
                    Details = null,
                    EmployeeId = EmployeeId,
                    Entity = nameof(MetersData),
                    EntityId = Id,
                    OwnerId = OwnerId,
                });
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> Create(CreateMetersDataDto dto, Guid? OwnerId, Guid? EmployeeId)
        {
            try
            {
                //TODO: валидация, можно ли создать для этого периода = проверить
                Guid Id = Guid.NewGuid();
                var entity = new MetersData
                {
                    Id = Id,
                    CreationDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    PaymentPeriodId = dto.PaymentPeriodId,
                    TariffId = dto.TariffId,
                    Value = dto.Value
                };

                //Проверка, что на данный период еще нет показаний счетчиков
                var checkExistEntity = await Get(dto.PaymentPeriodId, dto.TariffId);
                if (checkExistEntity != null)
                {
                    return false;
                }

                //Проверка, что статус периода = сбору показаний счетчиков
                //var period = await paymentPeriodService.Get(dto.PaymentPeriodId);
                //if (period.StatusId != Constants.PaymentPeriodStatus.CollectingReadings)
                //{
                //    return false;
                //}


                await db.MetersDatas.AddAsync(entity);
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto
                {
                    CreationDate = DateTime.Now,
                    Action = Constants.HistoryAction.Create,
                    Details = null,
                    EmployeeId = EmployeeId,
                    Entity = nameof(MetersData),
                    EntityId = Id,
                    OwnerId = OwnerId
                });
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IList<ViewMetersDataDto>> List(Guid FlatId, Guid PeriodId)
        {
            try
            {
                var entities = await db.MetersDatas
                    .Where(x => x.PaymentPeriodId == PeriodId)
                    .Include(x => x.PaymentPeriod)
                    .Include(x => x.Tariff)
                        .ThenInclude(x => x.Flat)
                        .ThenInclude(x => x.Building)
                        .ThenInclude(x => x.Street)
                    .Include(x => x.Tariff)
                    .ThenInclude(x => x.ServiceType)
                    .Where(x => x.Tariff.FlatId == FlatId)
                    .ToListAsync();

                var dtos = entities.Select(entity => new ViewMetersDataDto
                {
                    Id = entity.Id,
                    CreationDate = entity.CreationDate,
                    ModifyDate = entity.ModifyDate,
                    Address = $"{Constants.BaseWord.Street} {entity.Tariff.Flat.Building.Street.Name} {Constants.BaseWord.Building} {entity.Tariff.Flat.Building.Name} {Constants.BaseWord.Apartament} {entity.Tariff.Flat.ApartmentNumber}",
                    PaymentPeriodId = entity.PaymentPeriodId,
                    TariffId = entity.TariffId,
                    PaymentPeriod = entity.PaymentPeriod.Name,
                    Value = entity.Value,
                    ServiceType = entity.Tariff.ServiceType.Name
                }).ToList();
                
                return dtos;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IList<ViewMetersDataDto>> List()
        {
            try
            {
                var entities = await db.MetersDatas
                    .Include(x => x.PaymentPeriod)
                    .Include(x => x.Tariff)
                    .ThenInclude(x => x.Flat)
                    .ThenInclude(x => x.Building)
                    .ThenInclude(x => x.Street)
                    .Include(x => x.Tariff)
                    .ThenInclude(x => x.ServiceType)
                    .ToListAsync();

                var dtos = entities.Select(entity => new ViewMetersDataDto
                {
                    Id = entity.Id,
                    CreationDate = entity.CreationDate,
                    ModifyDate = entity.ModifyDate,
                    Address = $"{Constants.BaseWord.Street} {entity.Tariff.Flat.Building.Street.Name} {Constants.BaseWord.Building} {entity.Tariff.Flat.Building.Name} {Constants.BaseWord.Apartament} {entity.Tariff.Flat.ApartmentNumber}",
                    PaymentPeriodId = entity.PaymentPeriodId,
                    TariffId = entity.TariffId,
                    PaymentPeriod = entity.PaymentPeriod.Name,
                    Value = entity.Value,
                    ServiceType = entity.Tariff.ServiceType.Name
                }).ToList();

                return dtos;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IList<ViewMetersDataDto>> List(Guid periodId)
        {
            try
            {
                var entities = await db.MetersDatas
                    .Include(x => x.PaymentPeriod)
                    .Include(x => x.Tariff)
                    .ThenInclude(x => x.Flat)
                    .ThenInclude(x => x.Building)
                    .ThenInclude(x => x.Street)
                    .Include(x => x.Tariff)
                    .ThenInclude(x => x.ServiceType)
                    .Where(x => x.PaymentPeriodId == periodId)
                    .ToListAsync();

                var dtos = entities.Select(entity => new ViewMetersDataDto
                {
                    Id = entity.Id,
                    CreationDate = entity.CreationDate,
                    ModifyDate = entity.ModifyDate,
                    Address = $"{Constants.BaseWord.Street} {entity.Tariff.Flat.Building.Street.Name} {Constants.BaseWord.Building} {entity.Tariff.Flat.Building.Name} {Constants.BaseWord.Apartament} {entity.Tariff.Flat.ApartmentNumber}",
                    PaymentPeriodId = entity.PaymentPeriodId,
                    TariffId = entity.TariffId,
                    PaymentPeriod = entity.PaymentPeriod.Name,
                    Value = entity.Value,
                    ServiceType = entity.Tariff.ServiceType.Name
                }).ToList();

                return dtos;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ViewMetersDataDto> Get(Guid Id)
        {
            try
            {
                var entity = await db.MetersDatas
                    .Where(x => x.Id == Id)
                    .Include(x => x.PaymentPeriod)
                    .Include(x => x.Tariff)
                    .ThenInclude(x => x.Flat)
                    .ThenInclude(x => x.Building)
                    .ThenInclude(x => x.Street)
                    .Include(x => x.Tariff)
                    .ThenInclude(x => x.ServiceType)
                    .FirstOrDefaultAsync();

                var dto = new ViewMetersDataDto
                {
                    Id = entity.Id,
                    CreationDate = entity.CreationDate,
                    ModifyDate = entity.ModifyDate,
                    Address = $"{Constants.BaseWord.Street} {entity.Tariff.Flat.Building.Street.Name} {Constants.BaseWord.Building} {entity.Tariff.Flat.Building.Name} {Constants.BaseWord.Apartament} {entity.Tariff.Flat.ApartmentNumber}",
                    PaymentPeriodId = entity.PaymentPeriodId,
                    TariffId = entity.TariffId,
                    PaymentPeriod = entity.PaymentPeriod.Name,
                    Value = entity.Value,
                    ServiceType = entity.Tariff.ServiceType.Name
                };

                return dto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<ViewMetersDataDto> Get(Guid periodId, Guid tariffId)
        {
            try
            {
                var entity = await db.MetersDatas
                    .Where(x => x.PaymentPeriodId == periodId && x.TariffId == tariffId)
                    .Include(x => x.PaymentPeriod)
                    .Include(x => x.Tariff)
                    .ThenInclude(x => x.Flat)
                    .ThenInclude(x => x.Building)
                    .ThenInclude(x => x.Street)
                    .Include(x => x.Tariff)
                    .ThenInclude(x => x.ServiceType)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    return null;

                var dto = new ViewMetersDataDto
                {
                    Id = entity.Id,
                    CreationDate = entity.CreationDate,
                    ModifyDate = entity.ModifyDate,
                    Address = $"{Constants.BaseWord.Street} {entity.Tariff.Flat.Building.Street.Name} {Constants.BaseWord.Building} {entity.Tariff.Flat.Building.Name} {Constants.BaseWord.Apartament} {entity.Tariff.Flat.ApartmentNumber}",
                    PaymentPeriodId = entity.PaymentPeriodId,
                    TariffId = entity.TariffId,
                    PaymentPeriod = entity.PaymentPeriod.Name,
                    Value = entity.Value,
                    ServiceType = entity.Tariff.ServiceType.Name
                };

                return dto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
