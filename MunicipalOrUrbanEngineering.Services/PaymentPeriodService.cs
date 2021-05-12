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
    public class PaymentPeriodService
    {
        private readonly MUEContext db;
        private readonly HistoryService historyService;
        private readonly StatusService statusService;
        private readonly TariffSerivce tariffSerivce;
        private readonly ServiceBillService serviceBill;
        private readonly PaymentSheetService paymentSheetService;
        private readonly MetersDataService metersDataService;
        public PaymentPeriodService(MUEContext db)
        {
            this.db = db;
            historyService = new HistoryService(db);
            statusService = new StatusService(db);
            tariffSerivce = new TariffSerivce(db);
            serviceBill = new ServiceBillService(db);
            paymentSheetService = new PaymentSheetService(db);
            metersDataService = new MetersDataService(db);
        }

        private async Task<PaymentPeriod> GetEntity(Guid Id)
        {
            return await db.PaymentPeriods.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<bool> Delete(Guid Id, Guid? EmployeeId)
        {
            try
            {

                var entity = await GetEntity(Id);
                db.PaymentPeriods.Remove(entity);
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.Delete,
                    CreationDate = DateTime.MaxValue,
                    Details = null,
                    EmployeeId = EmployeeId,
                    Entity = nameof(PaymentPeriod),
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
        public async Task<bool> Create(CreatePaymentPeriodDto dto, Guid? Employeeid)
        {
            Guid Id = Guid.NewGuid();
            var MonthName = GetMonthName(dto.Month.Month);

            var entity = new PaymentPeriod
            {
                Id = Id,
                CreationDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                StartDate = GetStartMonth(dto.Month),
                EndDate = GetEndMonth(dto.Month),
                Name = $"{MonthName} {dto.Month.Year}",
                StatusId = Constants.PaymentPeriodStatus.New
            };
            //Gроверка, что такой период еще не сущствует (start, end)
            if (await Get(entity.StartDate.AddDays(1)) != null)
            {
                return false;
            }
            await db.PaymentPeriods.AddAsync(entity);
            await db.SaveChangesAsync();

            await historyService.Create(new CreateHistoryDto
            {
                Action = Constants.HistoryAction.Create,
                CreationDate = DateTime.Now,
                Details = null,
                EmployeeId = Employeeid,
                Entity = nameof(PaymentPeriod),
                EntityId = Id,
                OwnerId = null
            });
            return true;
        }

        public async Task<ViewPaymentPeriodDto> Get(Guid Id)
        {
            try
            {
               var entity = await db.PaymentPeriods
                    .Include(x => x.Status)
                    .FirstOrDefaultAsync(x => x.Id == Id);
               if (entity == null)
               {
                   return null;
               }
               
               return new ViewPaymentPeriodDto
               {
                   Id = entity.Id,
                   CreationDate = entity.CreationDate,
                   ModifyDate = entity.ModifyDate,
                   StartDate = entity.StartDate,
                   EndDate = entity.EndDate,
                   EnableClose = entity.StatusId == Constants.PaymentPeriodStatus.PaymentOfReceipts,
                   EnableCollectingReadings = entity.StatusId == Constants.PaymentPeriodStatus.New,
                   EnableFormationOfReceipts = entity.StatusId == Constants.PaymentPeriodStatus.CollectingReadings,
                   EnablePaymentOfReceipts = entity.StatusId == Constants.PaymentPeriodStatus.FormationOfReceipts,
                   Name = entity.Name,
                   Status = entity.Status.Name,
                   StatusId = entity.StatusId
               };

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<ViewPaymentPeriodDto> GetPreviousPeriod(Guid currentPeriodId)
        {
            try
            {
                var period = await Get(currentPeriodId);
                var PrevPeriod = await Get(period.StartDate.AddDays(-1));
                return PrevPeriod;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<ViewPaymentPeriodDto> Get(DateTime date)
        {
            try
            {
                var entity = await db.PaymentPeriods
                    .Include(x => x.Status)
                    .FirstOrDefaultAsync(x => x.StartDate.Date <= date.Date && x.EndDate.Date >= date.Date);
                if (entity == null)
                {
                    return null;
                }
                return new ViewPaymentPeriodDto
                {
                    Id = entity.Id,
                    CreationDate = entity.CreationDate,
                    ModifyDate = entity.ModifyDate,
                    StartDate = entity.StartDate,
                    EndDate = entity.EndDate,
                    EnableClose = entity.StatusId == Constants.PaymentPeriodStatus.PaymentOfReceipts,
                    EnableCollectingReadings = entity.StatusId == Constants.PaymentPeriodStatus.New,
                    EnableFormationOfReceipts = entity.StatusId == Constants.PaymentPeriodStatus.CollectingReadings,
                    EnablePaymentOfReceipts = entity.StatusId == Constants.PaymentPeriodStatus.FormationOfReceipts,
                    Name = entity.Name,
                    Status = entity.Status.Name,
                    StatusId = entity.StatusId
                    
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<IList<ViewPaymentPeriodDto>> List()
        {
            try
            {
                var entityList = await db.PaymentPeriods
                    .Include(x => x.Status)
                    .ToListAsync();
                return entityList.Select(entity => new ViewPaymentPeriodDto
                    {
                        Id = entity.Id,
                        CreationDate = entity.CreationDate,
                        ModifyDate = entity.ModifyDate,
                        StartDate = entity.StartDate,
                        EndDate = entity.EndDate,
                        EnableClose = entity.StatusId == Constants.PaymentPeriodStatus.PaymentOfReceipts,
                        EnableCollectingReadings = entity.StatusId == Constants.PaymentPeriodStatus.New,
                        EnableFormationOfReceipts = entity.StatusId == Constants.PaymentPeriodStatus.CollectingReadings,
                        EnablePaymentOfReceipts = entity.StatusId == Constants.PaymentPeriodStatus.FormationOfReceipts,
                        Name = entity.Name,
                        Status = entity.Status.Name
                    })
                    .OrderByDescending(x => x.StartDate)
                    .ToList();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<IList<ViewPaymentPeriodDto>> List(Guid StatusId)
        {
            try
            {
                var entityList = await db.PaymentPeriods
                    .Include(x => x.Status)
                    .Where(x => x.StatusId == StatusId)
                    .ToListAsync();
                return entityList.Select(entity => new ViewPaymentPeriodDto
                    {
                        Id = entity.Id,
                        CreationDate = entity.CreationDate,
                        ModifyDate = entity.ModifyDate,
                        StartDate = entity.StartDate,
                        EndDate = entity.EndDate,
                        EnableClose = entity.StatusId == Constants.PaymentPeriodStatus.PaymentOfReceipts,
                        EnableCollectingReadings = entity.StatusId == Constants.PaymentPeriodStatus.New,
                        EnableFormationOfReceipts = entity.StatusId == Constants.PaymentPeriodStatus.CollectingReadings,
                        EnablePaymentOfReceipts = entity.StatusId == Constants.PaymentPeriodStatus.FormationOfReceipts,
                        Name = entity.Name,
                        Status = entity.Status.Name
                    })
                    .OrderByDescending(x => x.StartDate)
                    .ToList();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }



        public async Task<bool> StartCollectingReadings(Guid PeriodId, Guid? EmployeeId)
        {
            try
            {
                var entity = await GetEntity(PeriodId);
                var statuses = await statusService.List(nameof(PaymentPeriod));
                if (entity.StatusId == Constants.PaymentPeriodStatus.New)
                {
                    var oldStatus = entity.StatusId;
                    entity.StatusId = Constants.PaymentPeriodStatus.CollectingReadings;
                    entity.ModifyDate = DateTime.Now;
                    await historyService.Create(new CreateHistoryDto
                    {
                        Action = Constants.HistoryAction.SetStatus,
                        CreationDate = DateTime.Now,
                        Details = $"From ({statuses.FirstOrDefault(x => x.Id == oldStatus).Name} To ({statuses.FirstOrDefault(x => x.Id == Constants.PaymentPeriodStatus.CollectingReadings).Name}))",
                        EmployeeId = EmployeeId,
                        Entity = nameof(PaymentPeriod),
                        EntityId = entity.Id,
                        OwnerId = null
                    });
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<bool> StartFormationReceipts(Guid PeriodId, Guid? EmployeeId)
        {
            try
            {
                var entity = await GetEntity(PeriodId);
                var statuses = await statusService.List(nameof(PaymentPeriod));
                if (entity.StatusId == Constants.PaymentPeriodStatus.CollectingReadings)
                {
                    var oldStatus = entity.StatusId;
                    entity.StatusId = Constants.PaymentPeriodStatus.FormationOfReceipts;
                    entity.ModifyDate = DateTime.Now;
                    await historyService.Create(new CreateHistoryDto
                    {
                        Action = Constants.HistoryAction.SetStatus,
                        CreationDate = DateTime.Now,
                        Details = $"From ({statuses.FirstOrDefault(x => x.Id == oldStatus).Name} To ({statuses.FirstOrDefault(x => x.Id == Constants.PaymentPeriodStatus.FormationOfReceipts)}))",
                        EmployeeId = EmployeeId,
                        Entity = nameof(PaymentPeriod),
                        EntityId = entity.Id,
                        OwnerId = null
                    });
                    var tariffByPeriodWithCounter = await tariffSerivce.List(entity.StartDate.AddDays(1), true);
                    var tariffByPeriodWithoutCounter = await tariffSerivce.List(entity.StartDate.AddDays(1), false);
                    await CalculateTariffWithCounter(PeriodId, EmployeeId, tariffByPeriodWithCounter, entity);
                    await CalculateTariffsWithoutCounter(PeriodId, EmployeeId, tariffByPeriodWithoutCounter, entity);
                    await paymentSheetService.CalculateAmountByPeriod(PeriodId, EmployeeId);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task CalculateTariffWithCounter(Guid PeriodId, Guid? EmployeeId, IList<ViewTariffDto> tariffByPeriodWithCounter, PaymentPeriod entity)
        {
            foreach (var tariff in tariffByPeriodWithCounter)
            {
                var data = await metersDataService.Get(PeriodId, tariff.Id);
                if (data == null)
                {
                    await metersDataService.Create(new CreateMetersDataDto
                    {
                        Value = tariff.DefaultData,
                        PaymentPeriodId = PeriodId,
                        TariffId = tariff.Id
                    }, null, EmployeeId);
                    data = await metersDataService.Get(PeriodId, tariff.Id);
                }

                var SheetId = await paymentSheetService.GetId(PeriodId, tariff.FlatId);
                if (!SheetId.HasValue)
                {
                    SheetId = await paymentSheetService.Create(new CreatePaymentSheetDto
                    {
                        PaymentPeriodId = PeriodId,
                        Comment = "Создано при смене статуса периода",
                        FlatId = tariff.FlatId,
                        Name = $" Счет на {entity.Name} для {tariff.Flat}"
                    }, EmployeeId);
                }

                var servicebillId = await serviceBill.GetId(tariff.Id, SheetId.Value);
                if (servicebillId == null)
                {
                    var serviceBilldto = new CreateServiceBillDto
                    {
                        Comment = "Создано при смене статуса периода",
                        PaymentSheetId = SheetId.Value,
                        TariffId = tariff.Id,
                        Value = data.Value * tariff.Value
                    };
                    await serviceBill.Create(serviceBilldto, EmployeeId);
                }

            }
        }

        private async Task CalculateTariffsWithoutCounter(Guid PeriodId, Guid? EmployeeId, IList<ViewTariffDto> tariffByPeriodWithoutCounter, PaymentPeriod entity)
        {
            foreach (var tariff in tariffByPeriodWithoutCounter)
            {
                var SheetId = await paymentSheetService.GetId(PeriodId, tariff.FlatId);
                if (!SheetId.HasValue)
                {
                    SheetId = await paymentSheetService.Create(new CreatePaymentSheetDto
                    {
                        PaymentPeriodId = PeriodId,
                        Comment = "Создано при смене статуса периода",
                        FlatId = tariff.FlatId,
                        Name = $" Счет на {entity.Name} для {tariff.Flat}"
                    }, EmployeeId);
                }

                var servicebillId = await serviceBill.GetId(tariff.Id, SheetId.Value);
                if (servicebillId == null)
                {
                    await serviceBill.Create(new CreateServiceBillDto
                    {
                        Comment = "Создано при смене статуса периода",
                        PaymentSheetId = SheetId.Value,
                        TariffId = tariff.Id,
                        Value = tariff.Value * tariff.DefaultData
                    }, EmployeeId);
                }
            }
        }

        public async Task<bool> StartPaymentOfReceipts(Guid PeriodId, Guid? EmployeeId)
        {
            try
            {
                var entity = await GetEntity(PeriodId);
                var statuses = await statusService.List(nameof(PaymentPeriod));
                if (entity.StatusId == Constants.PaymentPeriodStatus.FormationOfReceipts)
                {
                    var oldStatus = entity.StatusId;
                    entity.StatusId = Constants.PaymentPeriodStatus.PaymentOfReceipts;
                    entity.ModifyDate = DateTime.Now;
                    await historyService.Create(new CreateHistoryDto
                    {
                        Action = Constants.HistoryAction.SetStatus,
                        CreationDate = DateTime.Now,
                        Details = $"From ({statuses.FirstOrDefault(x => x.Id == oldStatus).Name} To ({statuses.FirstOrDefault(x => x.Id == Constants.PaymentPeriodStatus.PaymentOfReceipts)}))",
                        EmployeeId = EmployeeId,
                        Entity = nameof(PaymentPeriod),
                        EntityId = entity.Id,
                        OwnerId = null
                    });

                    await paymentSheetService.StartAcceptingPayments(PeriodId, EmployeeId);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<bool> ClosePeriod(Guid PeriodId, Guid? EmployeeId)
        {
            try
            {
                var entity = await GetEntity(PeriodId);
                var statuses = await statusService.List(nameof(PaymentPeriod));
                if (entity.StatusId == Constants.PaymentPeriodStatus.PaymentOfReceipts)
                {
                    var oldStatus = entity.StatusId;
                    entity.StatusId = Constants.PaymentPeriodStatus.Close;
                    entity.ModifyDate = DateTime.Now;
                    await historyService.Create(new CreateHistoryDto
                    {
                        Action = Constants.HistoryAction.SetStatus,
                        CreationDate = DateTime.Now,
                        Details = $"From ({statuses.FirstOrDefault(x => x.Id == oldStatus).Name} To ({statuses.FirstOrDefault(x => x.Id == Constants.PaymentPeriodStatus.Close)}))",
                        EmployeeId = EmployeeId,
                        Entity = nameof(PaymentPeriod),
                        EntityId = entity.Id,
                        OwnerId = null
                    });
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        private static string GetMonthName(int month)
        {
            switch (month)
            {
                case 1:
                    return "Январь";
                case 2:
                    return "Февраль";
                case 3:
                    return "Март";
                case 4:
                    return "Апрель";
                case 5:
                    return "Май";
                case 6:
                    return "Июнь";
                case 7:
                    return "Июль";
                case 8:
                    return "Август";
                case 9:
                    return "Сентябрь";
                case 10:
                    return "Октябрь";
                case 11:
                    return "Ноябрь";
                case 12:
                    return "Декабрь";
                default:
                    return "Default";
            }

        }
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
