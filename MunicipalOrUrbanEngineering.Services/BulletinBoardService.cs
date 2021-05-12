using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MunicipalOrUrbanEngineering.DataTransferObjects;
using MunicipalOrUrbanEngineering.DataTransferObjects.Address;
using MunicipalOrUrbanEngineering.Entities;

namespace MunicipalOrUrbanEngineering.Services
{
    public class BulletinBoardService
    {
        private readonly MUEContext db;
        private readonly HistoryService historyService;
        private readonly EmployeeService employeeService;
        private readonly BuildingService buildingService;
        private readonly OwnerService ownerService;
        
        public BulletinBoardService(MUEContext db)
        {
            this.db = db;
            historyService = new HistoryService(db);
            employeeService = new EmployeeService(db);
            buildingService = new BuildingService(db);
            ownerService = new OwnerService(db);
        }

        private async Task<BulletinBoardItem> GetEntity(Guid Id)
        {
            try
            {
                return await db.BulletinBoardItems.FirstOrDefaultAsync(x => x.Id == Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> Create(CreateBulletinBoardItemDto dto, Guid? EmployeeId)
        {
            try
            {
                Guid Id = Guid.NewGuid();
                var BulletinBoardItem = new BulletinBoardItem
                {
                    Id = Id,
                    CreationDate = DateTime.Now,
                    CreatorId = dto.CreatorId,
                    Description = dto.Description,
                    ShortDescription = dto.ShortDescription,
                    ModifyDate = DateTime.Now,
                    ShowStart = dto.ShowStart,
                    ShowEnd = dto.ShowEnd,
                    ToFlatId = dto.ToFlatId,
                    ToBuildingId = dto.ToBuildingId,
                    ToStreetId = dto.ToStreetId,
                    ToOwnerId = dto.ToOwnerId,
                    ToAllUsers = dto.ToAllUsers,
                    Title = dto.Title
                };
                await db.BulletinBoardItems.AddAsync(BulletinBoardItem);
                await db.SaveChangesAsync();
                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.Create,
                    CreationDate = DateTime.Now,
                    Details = null,
                    EmployeeId = EmployeeId,
                    Entity = nameof(BulletinBoardItem),
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

        public async Task<bool> Delete(Guid Id, Guid? EmployeeId)
        {
            try
            {
                var entity = await GetEntity(Id);
                db.BulletinBoardItems.Remove(entity);
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.Delete,
                    CreationDate = DateTime.Now,
                    Details = null,
                    EmployeeId = EmployeeId,
                    Entity = nameof(BulletinBoardItem),
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

        public async Task<ViewBulletinBoardItemDto> GetView(Guid Id)
        {
            try
            {
                var entity = await GetEntity(Id);
                var CreatorFullName = "Admin";
                if (entity.CreatorId !=  Guid.Empty)
                {
                    var creator = await employeeService.Get(entity.CreatorId);
                    CreatorFullName = creator.Fullname;
                }

                string Recipient = string.Empty;
                if (entity.ToAllUsers)
                {
                    Recipient = Constants.BaseWord.AllUsers;
                }

                if (entity.ToBuildingId.HasValue)
                {
                    var building = await buildingService.GetBuilding(entity.ToBuildingId.Value);
                    Recipient =
                        $"{Constants.BaseWord.Street} {building.Street} {Constants.BaseWord.Building} {building.Name}";
                }

                if (entity.ToStreetId.HasValue)
                {
                    var street = await buildingService.GetBuilding(entity.ToStreetId.Value);
                    Recipient = $"{Constants.BaseWord.Street} {street.Name}";
                }

                if (entity.ToFlatId.HasValue)
                {
                    var flat = await buildingService.GetFlat(entity.ToFlatId.Value);
                    Recipient = $"{flat.FullAddress}";
                }

                if (entity.ToOwnerId.HasValue)
                {
                    var owner = await ownerService.Get(entity.ToOwnerId.Value);
                    Recipient = $"{owner.Fullname}";
                }
                
                return new ViewBulletinBoardItemDto
                {
                    Id = entity.Id,
                    Recipient = Recipient,
                    CreationDate = entity.CreationDate,
                    ModifyDate = entity.ModifyDate,
                    Description = entity.Description,
                    ShortDescription = entity.ShortDescription,
                    Title = entity.Title,
                    CreatorFullName = CreatorFullName,
                    Start = $"{entity.ShowStart.Day}.{entity.ShowStart.Month}.{entity.ShowStart.Year}",
                    End = $"{entity.ShowEnd.Day}.{entity.ShowEnd.Month}.{entity.ShowEnd.Year}"
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IList<ViewBulletinBoardItemDto>> GetActual(IList<ViewFlatDto> flatsDtos, Guid? OwnerId)
        {
            try
            {
                var flatIds = flatsDtos.Select(x => x.Id).ToList();
                var buildingIds = flatsDtos.Select(x => x.BuildingId).ToList();
                var streetIds = flatsDtos.Select(x => x.StreetId).ToList();

                var employees = await employeeService.List();
                var AllShowBulletinBoard = await db.BulletinBoardItems
                    .Where(x => (x.ShowStart <= DateTime.Now) && (x.ShowEnd >= DateTime.Now))
                    .ToListAsync();
                var result = new List<ViewBulletinBoardItemDto>();
                //TODO
                result = AllShowBulletinBoard.Where(x => x.ToAllUsers
                || ((x.ToOwnerId != null) && (x.ToOwnerId == OwnerId))
                    || ((x.ToFlatId != null) && (flatIds.Contains((Guid)x.ToFlatId)))
                    || ((x.ToStreetId != null) && (buildingIds.Contains((Guid)x.ToStreetId)))
                    || ((x.ToBuildingId != null) && (buildingIds.Contains((Guid)x.ToBuildingId)))
                    )
                    .Select(item => new ViewBulletinBoardItemDto
                    {
                        CreationDate = item.CreationDate,
                        Description = item.Description,
                        Id = item.Id,
                        ModifyDate = item.ModifyDate,
                        ShortDescription = item.ShortDescription,
                        Start = $"{item.ShowStart.Day}.{item.ShowStart.Month}.{item.ShowStart.Year}",
                        End = $"{item.ShowEnd.Day}.{item.ShowEnd.Month}.{item.ShowEnd.Year}",
                        Title = item.Title,
                        CreatorFullName = item.CreatorId == Guid.Empty ?
                            "Admin" 
                            : employees.Select(emp => emp.Fullname).FirstOrDefault()
                    })
                    .ToList();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IList<ViewBulletinBoardItemDto>> List()
        {
            try
            {
                var employees = await employeeService.List();
                var flats = await buildingService.ListFlat();
                var buildings = await buildingService.ListBuilding();
                var streets = await buildingService.ListStreet();
                var bulletinboardEntities= await db.BulletinBoardItems
                    .Include(x => x.Creator)
                    .Include(x => x.ToOwner)
                    .ToListAsync();
               var bulletinboard = bulletinboardEntities.Select(bi => new ViewBulletinBoardItemDto
                {
                    Id = bi.Id,
                    CreationDate = bi.CreationDate,
                    Description = bi.Description,
                    ModifyDate = bi.ModifyDate,
                    Title = bi.Title,
                    ShortDescription = bi.ShortDescription,
                    Start = $"{bi.ShowStart.Day}.{bi.ShowStart.Month}.{bi.ShowStart.Year}",
                    End = $"{bi.ShowEnd.Day}.{bi.ShowEnd.Month}.{bi.ShowEnd.Year}",
                    CreatorId = bi.CreatorId,
                    CreatorFullName = bi.CreatorId == Guid.Empty
                        ? "Admin"
                        : $"{bi.Creator.LastName} {bi.Creator.FirstName} {bi.Creator.MiddleName}",
                    Recipient = GetRecipient(bi.ToAllUsers, streets, buildings, flats, bi.ToOwner, bi.ToStreetId, bi.ToBuildingId, bi.ToFlatId)
               }).ToList();
               
                return bulletinboard;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private string GetRecipient(bool ToAllUsers, IList<ViewStreetDto> streets, IList<ViewBuildingDto> buildings, IList<ViewFlatDto> flats, Owner owner, Guid? street, Guid? building, Guid? flat)
        {
            if (ToAllUsers)
            {
                return $"{Constants.BaseWord.AllUsers}";
            }
            if (street != null)
            {
                return $"{Constants.BaseWord.Street} {streets.FirstOrDefault(x =>x.Id == street.Value).Name}";
            }
            if (building != null)
            {
                var b = buildings.FirstOrDefault(x => x.Id == building.Value);
                return $"{Constants.BaseWord.Street} {b.Street} {Constants.BaseWord.Building} {b.Name}";
            }
            if (flat != null)
            {
                var f = flats.FirstOrDefault(x => x.Id == flat.Value);
                return $"{f.FullAddress}";
            }

            if (owner != null)
            {
                return $"{owner.LastName} {owner.FirstName} {owner.MiddleName}";
            }
            return string.Empty;
        }
    }
}
