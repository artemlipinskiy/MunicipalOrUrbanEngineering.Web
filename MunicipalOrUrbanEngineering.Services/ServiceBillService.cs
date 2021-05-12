using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MunicipalOrUrbanEngineering.DataTransferObjects;
using MunicipalOrUrbanEngineering.DataTransferObjects.Payment;
using MunicipalOrUrbanEngineering.Entities;
using MunicipalOrUrbanEngineering.Services.Migrations;

namespace MunicipalOrUrbanEngineering.Services
{
    public class ServiceBillService
    {
        private readonly MUEContext db;
        private readonly HistoryService historyService;
        private readonly StatusService statusService;
        private readonly MetersDataService metersDataService;

        public ServiceBillService(MUEContext db)
        {
            this.db = db;
            historyService = new HistoryService(db);
            statusService = new StatusService(db);
            metersDataService = new MetersDataService(db);
        }


        public async Task<bool> SetStatus(Guid ServiceBillId, Guid StatusId, Guid? EmployeeId = null, Guid? OwnerId = null)
        {
            try
            {
                var statuses = await statusService.List(nameof(ServiceBill));
                var entity = await GetEntity(ServiceBillId);
                var oldStatus = statuses.FirstOrDefault(x => x.Id == entity.StatusId).Name;
                var newStatus = statuses.FirstOrDefault(x => x.Id == StatusId).Name;
                entity.StatusId = StatusId;
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.SetStatus,
                    CreationDate = DateTime.Now,
                    Details = $"From ({oldStatus}) to ({newStatus})",
                    EmployeeId = EmployeeId,
                    Entity = nameof(PaymentSheet),
                    EntityId = entity.Id,
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

        private async Task<ServiceBill> GetEntity(Guid Id)
        {
            return await db.ServiceBills
                .Include(x => x.Status)
                .Include(x => x.Tariff)
                .ThenInclude(x => x.ServiceType)
                .Include(x => x.PaymentSheet)
                .ThenInclude(x => x.PaymentPeriod)
                .FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<ViewServiceBillDto> Get(Guid Id)
        {
            try
            {
                var entity = await GetEntity(Id);
               
                var metersData = await metersDataService.List(entity.PaymentSheet.PaymentPeriodId);

                return new ViewServiceBillDto
                {
                    Id = entity.Id,
                    Comment = entity.Comment,
                    CreationDate = entity.CreationDate,
                    ModifyDate = entity.ModifyDate,
                    PaymentSheet = entity.PaymentSheet.Name,
                    Status = entity.Status.Name,
                    PaymentSheetId = entity.PaymentSheetId,
                    StatusId = entity.StatusId ?? Guid.Empty,
                    TariffId = entity.TariffId,
                    Value = entity.Value,
                    ServiceType = entity.Tariff.ServiceType.Name,
                    UnitName = entity.Tariff.ServiceType.UnitName,
                    Consume = entity.Tariff.ServiceType.IsCounterReadings
                        ? (metersData.FirstOrDefault(y => y.TariffId == entity.TariffId && y.PaymentPeriodId == entity.PaymentSheet.PaymentPeriodId)).Value
                        : entity.Tariff.DefaultData,
                    ValueUnit = entity.Tariff.Value,
                    HasCounter = entity.Tariff.ServiceType.IsCounterReadings ? "Да" : "Нет"
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }



        public async Task<Guid?> GetId(Guid tariffId, Guid SheetId)
        {
            try
            {
                var entity = await db.ServiceBills
                    .FirstOrDefaultAsync(x => x.TariffId == tariffId && x.PaymentSheetId == SheetId);
                if (entity == null)
                {
                    return null;
                }

                return entity.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<bool> Delete(Guid Id)
        {
            try
            {
                var entity = await GetEntity(Id);
                db.ServiceBills.Remove(entity);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Guid> Create(CreateServiceBillDto dto, Guid? employeeId = null, Guid? ownerId = null)
        {
            try
            {
                Guid Id = Guid.NewGuid();
                var entity = new ServiceBill
                {
                    Id = Id,
                    Comment = dto.Comment,
                    CreationDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    PaymentSheetId = dto.PaymentSheetId,
                    StatusId = Constants.ServiceBillStatus.ReadyForPayment,
                    TariffId = dto.TariffId,
                    Value = dto.Value
                };
                
                await db.ServiceBills.AddAsync(entity);
                await db.SaveChangesAsync();
                
                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.Create,
                    CreationDate = DateTime.Now,
                    Details = null,
                    EmployeeId = employeeId,
                    Entity = nameof(ServiceBill),
                    EntityId = Id,
                    OwnerId = ownerId
                });
                return Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IList<ViewServiceBillDto>> List(Guid sheetId)
        {
            try
            {
                var bills = await db.ServiceBills
                    .Include(x => x.Status)
                    .Include(x => x.Tariff)
                    .ThenInclude(x => x.ServiceType)
                    .Include(x => x.PaymentSheet)
                    .ThenInclude(x => x.PaymentPeriod)
                    .Where(x => x.PaymentSheetId == sheetId)
                    .ToListAsync();
                
                var firstbills = bills.FirstOrDefault();
                if (firstbills == null)
                {
                    return null;
                }
                var metersData = await metersDataService.List(firstbills.PaymentSheet.PaymentPeriodId);
                
                return bills.Select(x => new ViewServiceBillDto
                {
                    Id = x.Id,
                    Comment = x.Comment,
                    CreationDate = x.CreationDate,
                    ModifyDate = x.ModifyDate,
                    PaymentSheet = x.PaymentSheet.Name,
                    Status = x.Status.Name,
                    PaymentSheetId = x.PaymentSheetId,
                    StatusId = x.StatusId ?? Guid.Empty,
                    TariffId = x.TariffId,
                    Value = x.Value,
                    ServiceType = x.Tariff.ServiceType.Name,
                    UnitName = x.Tariff.ServiceType.UnitName,
                    Consume = x.Tariff.ServiceType.IsCounterReadings 
                        ? (metersData.FirstOrDefault(y => y.TariffId == x.TariffId && y.PaymentPeriodId == x.PaymentSheet.PaymentPeriodId)).Value 
                        : x.Tariff.DefaultData,
                    ValueUnit = x.Tariff.Value,
                    HasCounter = x.Tariff.ServiceType.IsCounterReadings ? "Да" : "Нет",
                    EnablePayment = x.StatusId.Value == Constants.ServiceBillStatus.ReadyForPayment
                }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> Pay(Guid id, Guid? employeeId = null)
        {
            try
            {
                await SetStatus(id, Constants.ServiceBillStatus.IsPaid, employeeId);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<bool> PayBySheetId(Guid id, Guid? employeeId = null)
        {
            try
            {
                var sheets = await db.ServiceBills.Where(x => x.PaymentSheetId == id).ToListAsync();
                foreach (var serviceBill in sheets)
                {
                    await SetStatus(serviceBill.Id, Constants.ServiceBillStatus.IsPaid, employeeId);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
