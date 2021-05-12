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
   public class PaymentSheetService
   {
       private readonly MUEContext db;
       private readonly HistoryService historyService;
       private readonly StatusService statusService;
       private readonly ServiceBillService serviceBillService;
       

       public PaymentSheetService(MUEContext db)
       {
           this.db = db;
           historyService = new HistoryService(db);
           statusService = new StatusService(db);
           serviceBillService = new ServiceBillService(db);
       }

       private async Task<PaymentSheet> GetEntity(Guid Id)
       {
           try
           {
               return await db.PaymentSheets
                   .Include(x => x.Flat)
                   .ThenInclude(x => x.Building)
                   .ThenInclude(x => x.Street)
                   .Include(x => x.PaymentPeriod)
                   .Include(x => x.Status)
                   .Include(x => x.ServiceBills)
                   .Include(x => x.Flat)
                   .ThenInclude(x=> x.Owner)
                   .FirstOrDefaultAsync(x => x.Id == Id);
           }
           catch (Exception e)
           {
               Console.WriteLine(e);
               throw;
           }
       }
       
        public async Task<bool> Delete(Guid Id, Guid? employeeId)
       {
           try
           {
               var entity = await GetEntity(Id);
               db.PaymentSheets.Remove(entity);
               await db.SaveChangesAsync();

               await historyService.Create(new CreateHistoryDto
               {
                   Action = Constants.HistoryAction.Delete,
                   CreationDate = DateTime.Now,
                   Details = null,
                   EmployeeId = employeeId,
                   Entity = nameof(PaymentSheet),
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

       public async Task<Guid> Create(CreatePaymentSheetDto dto, Guid? employeeId)
       {
           try
           {
                Guid Id = Guid.NewGuid();
                var entity = new PaymentSheet
                {
                    Id = Id,
                    Comment = dto.Comment,
                    FlatId = dto.FlatId,
                    Name = dto.Name,
                    PaymentPeriodId = dto.PaymentPeriodId,
                    StatusId = Constants.PaymentSheetStatus.InitializationProcess,
                    Amount = 0
                };
                await db.PaymentSheets.AddAsync(entity);

                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.Create,
                    CreationDate = DateTime.Now,
                    Details = null,
                    EmployeeId = employeeId,
                    Entity = nameof(PaymentSheet),
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

       public async Task<bool> StartAcceptingPayments(Guid periodId, Guid? employeeId)
       {
           try
           {
               var sheets = await db.PaymentSheets.Where(x => x.PaymentPeriodId == periodId).ToListAsync();
               foreach (var paymentSheet in sheets)
               {
                   await SetStatus(paymentSheet.Id, Constants.PaymentSheetStatus.ReadyForPayment, employeeId);
               }

               return true;
           }
           catch (Exception e)
           {
               Console.WriteLine(e);
               throw;
           }
       }
       
       public async Task<bool> CalculateAmountByPeriod(Guid periodId, Guid? employeeId)
       {
           try
           {
               var sheets = await db.PaymentSheets.Where(x => x.PaymentPeriodId == periodId).ToListAsync();
               foreach (var paymentSheet in sheets)
               {
                   await CalculateAmount(paymentSheet.Id, employeeId);
               }

               return true;
           }
           catch (Exception e)
           {
               Console.WriteLine(e);
               throw;
           }
       }

       public async Task<bool> CalculateAmount(Guid sheetId, Guid? employeeId)
       {
           try
           {
               var entity = await GetEntity(sheetId);
               var amount = entity.ServiceBills.Sum(x => x.Value);
               entity.Amount = amount;
               
               await db.SaveChangesAsync();

               await historyService.Create(new CreateHistoryDto
               {
                   Action = Constants.HistoryAction.Update,
                   CreationDate = DateTime.Now,
                   Details = "Итоговый расчет",
                   EmployeeId = employeeId,
                   Entity = nameof(PaymentSheet),
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

        public async Task<ViewPaymentSheetDto> Get(Guid id)
       {
           try
           {
               var entity = await GetEntity(id);
               if (entity == null)
               {
                   return null;
               }

               return new ViewPaymentSheetDto
               {
                   Id = entity.Id,
                   FlatId = entity.FlatId.Value,
                   Flat =
                       $"{Constants.BaseWord.Street} {entity.Flat.Building.Street.Name} {Constants.BaseWord.Building} {entity.Flat.Building.Name} {Constants.BaseWord.Apartament} {entity.Flat.ApartmentNumber}",
                   Amount = entity.Amount,
                   Comment = entity.Comment,
                   Name = entity.Name,
                   PaymentPeriodId = entity.PaymentPeriodId,
                   PaymentPeriod = entity.PaymentPeriod.Name,
                   StatusId = entity.StatusId,
                   Status = entity.Status.Name,
                   EnableDetails = entity.StatusId != Constants.PaymentSheetStatus.InitializationProcess,
                   EnablePayment = entity.StatusId == Constants.PaymentSheetStatus.ReadyForPayment || entity.StatusId == Constants.PaymentSheetStatus.IsPartiallyPaid,
                   Owner = entity.Flat.OwnerId.HasValue ? $"{entity.Flat.Owner.LastName} {entity.Flat.Owner.FirstName} {entity.Flat.Owner.MiddleName}" : string.Empty
               };
           }
           catch (Exception e)
           {
               Console.WriteLine(e);
               throw;
           }
       }
       public async Task<ViewPaymentSheetDto> Get(Guid PaymentPeriodId, Guid FlatId)
       {
           try
           {
               var entity = await db.PaymentSheets
                   .Include(x => x.Flat)
                   .ThenInclude(x => x.Building)
                   .ThenInclude(x => x.Street)
                   .Include(x => x.PaymentPeriod)
                   .Include(x => x.Status)
                   .Include(x => x.Flat)
                   .ThenInclude(x => x.Owner)
                   .FirstOrDefaultAsync(x => x.PaymentPeriodId == PaymentPeriodId && x.FlatId != null && x.FlatId == FlatId);
               if (entity == null)
               {
                   return null;
               }

               return new ViewPaymentSheetDto
               {
                   Id = entity.Id,
                   FlatId = entity.FlatId.Value,
                   Flat =
                       $"{Constants.BaseWord.Street} {entity.Flat.Building.Street.Name} {Constants.BaseWord.Building} {entity.Flat.Building.Name} {Constants.BaseWord.Apartament} {entity.Flat.ApartmentNumber}",
                   Amount = entity.Amount,
                   Comment = entity.Comment,
                   Name = entity.Name,
                   PaymentPeriodId = entity.PaymentPeriodId,
                   PaymentPeriod = entity.PaymentPeriod.Name,
                   StatusId = entity.StatusId,
                   Status = entity.Status.Name,
                   EnableDetails = entity.StatusId != Constants.PaymentSheetStatus.InitializationProcess,
                   EnablePayment = entity.StatusId == Constants.PaymentSheetStatus.ReadyForPayment || entity.StatusId == Constants.PaymentSheetStatus.IsPartiallyPaid,
                   Owner = entity.Flat.OwnerId.HasValue ? $"{entity.Flat.Owner.LastName} {entity.Flat.Owner.FirstName} {entity.Flat.Owner.MiddleName}" : string.Empty
               };
           }
           catch (Exception e)
           {
               Console.WriteLine(e);
               throw;
           }
       }
       public async Task<Guid?> GetId(Guid PaymentPeriodId, Guid FlatId)
       {
           try
           {
               var entity = await db.PaymentSheets
                   .FirstOrDefaultAsync(x => x.PaymentPeriodId == PaymentPeriodId && x.FlatId != null && x.FlatId == FlatId);
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
       public async Task<bool> SetStatus(Guid PaymentSheetId, Guid StatusId, Guid? EmployeeId = null, Guid? OwnerId = null)
       {
           try
           {
               var statuses = await statusService.List(nameof(PaymentSheet));
               var entity = await GetEntity(PaymentSheetId);
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

       public async Task<bool> CheckPay(Guid sheetId, Guid? employeeId)
       {
           try
           {
               var biils = await serviceBillService.List(sheetId);
               if (biils.All(x => x.StatusId == Constants.ServiceBillStatus.IsPaid))
               {
                  await SetStatus(sheetId, Constants.PaymentSheetStatus.IsPaid, employeeId);
                  return true;
               }
               if (biils.All(x => x.StatusId == Constants.ServiceBillStatus.ReadyForPayment))
               {
                   await SetStatus(sheetId, Constants.PaymentSheetStatus.ReadyForPayment, employeeId);
                   return true;
               }

               await SetStatus(sheetId, Constants.PaymentSheetStatus.IsPartiallyPaid, employeeId);
               return true;
           }
           catch (Exception e)
           {
               Console.WriteLine(e);
               throw;
           }
       }
       public async Task<IList<ViewPaymentSheetDto>> ListByFlat(Guid flatId)
       {
           try
           {
               var sheets = await db.PaymentSheets
                   .Include(x => x.Flat)
                   .ThenInclude(x => x.Building)
                   .ThenInclude(x => x.Street)
                   .Include(x => x.PaymentPeriod)
                   .Include(x => x.Status)
                   .Include(x => x.ServiceBills)
                   .Include(x => x.Flat)
                   .ThenInclude(x => x.Owner)
                   .Where(x => x.FlatId == flatId)
                   .ToListAsync();

               return sheets.Select(x => new ViewPaymentSheetDto
               {
                   Id = x.Id,
                   Amount = x.Amount,
                   Comment = x.Comment,
                   FlatId = x.FlatId.Value,
                   Flat = $"{Constants.BaseWord.Street} {x.Flat.Building.Street.Name} {Constants.BaseWord.Building} {x.Flat.Building.Name} {Constants.BaseWord.Apartament} {x.Flat.ApartmentNumber}",
                   Name = x.Name,
                   PaymentPeriod = x.PaymentPeriod.Name,
                   PaymentPeriodId = x.PaymentPeriodId,
                   Status = x.Status.Name,
                   StatusId = x.StatusId,
                   EnableDetails = x.StatusId != Constants.PaymentSheetStatus.InitializationProcess,
                   EnablePayment = x.StatusId == Constants.PaymentSheetStatus.ReadyForPayment || x.StatusId == Constants.PaymentSheetStatus.IsPartiallyPaid,
                   Owner = x.Flat.OwnerId.HasValue ? $"{x.Flat.Owner.LastName} {x.Flat.Owner.FirstName} {x.Flat.Owner.MiddleName}" : string.Empty

               }).ToList();
           }
           catch (Exception e)
           {
               Console.WriteLine(e);
               throw;
           }
       }
       public async Task<IList<ViewPaymentSheetDto>> ListByFlats(IList<Guid> flatIds)
       {
           try
           {
               var sheets = await db.PaymentSheets
                   .Include(x => x.Flat)
                   .ThenInclude(x => x.Building)
                   .ThenInclude(x => x.Street)
                   .Include(x => x.PaymentPeriod)
                   .Include(x => x.Status)
                   .Include(x => x.ServiceBills)
                   .Include(x => x.Flat)
                   .ThenInclude(x => x.Owner)
                   .Where(x => flatIds.Contains(x.FlatId.Value))
                   .ToListAsync();

               return sheets.Select(x => new ViewPaymentSheetDto
               {
                   Id = x.Id,
                   Amount = x.Amount,
                   Comment = x.Comment,
                   FlatId = x.FlatId.Value,
                   Flat = $"{Constants.BaseWord.Street} {x.Flat.Building.Street.Name} {Constants.BaseWord.Building} {x.Flat.Building.Name} {Constants.BaseWord.Apartament} {x.Flat.ApartmentNumber}",
                   Name = x.Name,
                   PaymentPeriod = x.PaymentPeriod.Name,
                   PaymentPeriodId = x.PaymentPeriodId,
                   Status = x.Status.Name,
                   StatusId = x.StatusId,
                   EnableDetails = x.StatusId != Constants.PaymentSheetStatus.InitializationProcess,
                   EnablePayment = x.StatusId == Constants.PaymentSheetStatus.ReadyForPayment || x.StatusId == Constants.PaymentSheetStatus.IsPartiallyPaid,
                   Owner = x.Flat.OwnerId.HasValue ? $"{x.Flat.Owner.LastName} {x.Flat.Owner.FirstName} {x.Flat.Owner.MiddleName}" : string.Empty

               }).ToList();
           }
           catch (Exception e)
           {
               Console.WriteLine(e);
               throw;
           }
       }
       public async Task<IList<ViewPaymentSheetDto>> ListByStatus(Guid statusId)
       {
           try
           {
               var sheets = await db.PaymentSheets
                   .Include(x => x.Flat)
                   .ThenInclude(x => x.Building)
                   .ThenInclude(x => x.Street)
                   .Include(x => x.PaymentPeriod)
                   .Include(x => x.Status)
                   .Include(x => x.ServiceBills)
                   .Include( x=> x.Flat)
                   .ThenInclude(x => x.Owner)
                   .Where(x => x.StatusId == statusId)
                   .ToListAsync();

               return sheets.Select(x => new ViewPaymentSheetDto
               {
                   Id = x.Id,
                   Amount = x.Amount,
                   Comment = x.Comment,
                   FlatId = x.FlatId.Value,
                   Flat = $"{Constants.BaseWord.Street} {x.Flat.Building.Street.Name} {Constants.BaseWord.Building} {x.Flat.Building.Name} {Constants.BaseWord.Apartament} {x.Flat.ApartmentNumber}",
                   Name = x.Name,
                   PaymentPeriod = x.PaymentPeriod.Name,
                   PaymentPeriodId = x.PaymentPeriodId,
                   Status = x.Status.Name,
                   StatusId = x.StatusId,
                   EnableDetails = x.StatusId != Constants.PaymentSheetStatus.InitializationProcess,
                   EnablePayment = x.StatusId == Constants.PaymentSheetStatus.ReadyForPayment || x.StatusId == Constants.PaymentSheetStatus.IsPartiallyPaid,
                   Owner = x.Flat.OwnerId.HasValue ? $"{x.Flat.Owner.LastName} {x.Flat.Owner.FirstName} {x.Flat.Owner.MiddleName}" : string.Empty

               }).ToList();
           }
           catch (Exception e)
           {
               Console.WriteLine(e);
               throw;
           }
       }
        public async Task<IList<ViewPaymentSheetDto>> ListByStatusAndPeriod(Guid statusId, Guid periodId)
        {
            try
            {
                var sheets = await db.PaymentSheets
                    .Include(x => x.Flat)
                    .ThenInclude(x => x.Building)
                    .ThenInclude(x => x.Street)
                    .Include(x => x.PaymentPeriod)
                    .Include(x => x.Status)
                    .Include(x => x.ServiceBills)
                    .Include(x => x.Flat)
                    .ThenInclude(x => x.Owner)
                    .Where(x => x.StatusId == statusId && x.PaymentPeriodId == periodId)
                    .ToListAsync();

                return sheets.Select(x => new ViewPaymentSheetDto
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    Comment = x.Comment,
                    FlatId = x.FlatId.Value,
                    Flat = $"{Constants.BaseWord.Street} {x.Flat.Building.Street.Name} {Constants.BaseWord.Building} {x.Flat.Building.Name} {Constants.BaseWord.Apartament} {x.Flat.ApartmentNumber}",
                    Name = x.Name,
                    PaymentPeriod = x.PaymentPeriod.Name,
                    PaymentPeriodId = x.PaymentPeriodId,
                    Status = x.Status.Name,
                    StatusId = x.StatusId,
                    EnableDetails = x.StatusId != Constants.PaymentSheetStatus.InitializationProcess,
                    EnablePayment = x.StatusId == Constants.PaymentSheetStatus.ReadyForPayment || x.StatusId == Constants.PaymentSheetStatus.IsPartiallyPaid,
                    Owner = x.Flat.OwnerId.HasValue ? $"{x.Flat.Owner.LastName} {x.Flat.Owner.FirstName} {x.Flat.Owner.MiddleName}" : string.Empty

                }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<IList<ViewPaymentSheetDto>> ListByPeriod(Guid periodId)
        {
            try
            {
                var sheets = await db.PaymentSheets
                    .Include(x => x.Flat)
                    .ThenInclude(x => x.Building)
                    .ThenInclude(x => x.Street)
                    .Include(x => x.PaymentPeriod)
                    .Include(x => x.Status)
                    .Include(x => x.ServiceBills)
                    .Include(x => x.Flat)
                    .ThenInclude(x => x.Owner)
                    .Where(x => x.PaymentPeriodId == periodId)
                    .ToListAsync();

                return sheets.Select(x => new ViewPaymentSheetDto
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    Comment = x.Comment,
                    FlatId = x.FlatId.Value,
                    Flat = $"{Constants.BaseWord.Street} {x.Flat.Building.Street.Name} {Constants.BaseWord.Building} {x.Flat.Building.Name} {Constants.BaseWord.Apartament} {x.Flat.ApartmentNumber}",
                    Name = x.Name,
                    PaymentPeriod = x.PaymentPeriod.Name,
                    PaymentPeriodId = x.PaymentPeriodId,
                    Status = x.Status.Name,
                    StatusId = x.StatusId,
                    EnableDetails = x.StatusId != Constants.PaymentSheetStatus.InitializationProcess,
                    EnablePayment = x.StatusId == Constants.PaymentSheetStatus.ReadyForPayment || x.StatusId == Constants.PaymentSheetStatus.IsPartiallyPaid,
                    Owner =x.Flat.OwnerId.HasValue ? $"{x.Flat.Owner.LastName} {x.Flat.Owner.FirstName} {x.Flat.Owner.MiddleName}" : string.Empty

                }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> Pay(Guid paymentsheetId, Guid? employeeId = null)
        {
            try
            {
               await SetStatus(paymentsheetId, Constants.PaymentSheetStatus.IsPaid, employeeId);
               await serviceBillService.PayBySheetId(paymentsheetId, employeeId);
               return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> PartPay(Guid serviceBillId, Guid? employeeId = null)
        {
            try
            {
                var serviceBill = await serviceBillService.Get(serviceBillId);
                await serviceBillService.SetStatus(serviceBill.Id, Constants.ServiceBillStatus.IsPaid, employeeId);
                await CheckPay(serviceBill.PaymentSheetId, employeeId);
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
