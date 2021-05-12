using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MunicipalOrUrbanEngineering.DataTransferObjects;
using MunicipalOrUrbanEngineering.DataTransferObjects.Address;
using MunicipalOrUrbanEngineering.Entities;

namespace MunicipalOrUrbanEngineering.Services
{
    public class BuildingService
    {
        private readonly MUEContext db;
        private readonly HistoryService historyService;

        public BuildingService(MUEContext db)
        {
            this.db = db;
            historyService = new HistoryService(db);
        }

        #region Entity
        private async Task<Street> GetStreetEntityAsync(Guid Id)
        {
            try
            {
                return await db.Streets.FirstOrDefaultAsync(x => x.Id == Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private async Task<Building> GetBuildingEntityAsync(Guid Id)
        {
            try
            {
                return await db.Buildings.FirstOrDefaultAsync(x => x.Id == Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private async Task<Flat> GetFlatEntityAsync(Guid Id)
        {
            try
            {
                return await db.Flats.Include(flat => flat.Owner).FirstOrDefaultAsync(x => x.Id == Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }



        #endregion

        private string GetDetailsUpdate(Flat fromdb, UpdateFlatDto dto)
        {
            try
            {
                if (fromdb.ApartmentNumber != dto.ApartmentNumber)
                {
                    return $"Property ({nameof(Flat.ApartmentNumber)}): From {fromdb.ApartmentNumber} to {dto.ApartmentNumber}";
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private string GetDetailsUpdate(Street fromdb, UpdateStreetDto dto)
        {
            try
            {
                var Details = string.Empty;
                if (fromdb.Name != dto.Name)
                {
                    Details += $"Property ({nameof(Street.Name)}): From {fromdb.Name} to {dto.Name} \n";
                }

                if (fromdb.Description != dto.Description)
                {
                    Details += $"Property ({nameof(Street.Description)}): From {fromdb.Description} to {dto.Description} \n";
                }
                return Details;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private string GetDetailsUpdate(Building fromdb, UpdateBuildingDto dto)
        {
            try
            {
                var Details = string.Empty;
                if (fromdb.Name != dto.Name)
                {
                    Details += $"Property ({nameof(Street.Name)}): From {fromdb.Name} to {dto.Name} \n";
                }

                if (fromdb.Description != dto.Description)
                {
                    Details += $"Property ({nameof(Street.Description)}): From {fromdb.Description} to {dto.Description} \n";
                }
                return Details;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> Update(UpdateFlatDto dto, Guid? employeeId = null)
        {
            try
            {
                var entity = await GetFlatEntityAsync(dto.Id);
                entity.ModifyDate = DateTime.Now;
                var Details = GetDetailsUpdate(entity, dto);

                entity.ApartmentNumber = dto.ApartmentNumber;
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.Update,
                    CreationDate = DateTime.Now,
                    Details = Details,
                    EmployeeId = employeeId,
                    EntityId = entity.Id,
                    Entity = nameof(Flat)
                });

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<bool> Update(UpdateStreetDto dto, Guid? employeeId = null)
        {
            try
            {
                var entity = await GetStreetEntityAsync(dto.Id);
                entity.ModifyDate = DateTime.Now;
                var Details = GetDetailsUpdate(entity, dto);

                entity.Name = dto.Name;
                entity.Description = dto.Description;
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.Update,
                    CreationDate = DateTime.Now,
                    Details = Details,
                    EmployeeId = employeeId,
                    EntityId = entity.Id,
                    Entity = nameof(Street)
                });

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<bool> Update(UpdateBuildingDto dto, Guid? employeeId = null)
        {
            try
            {
                var entity = await GetBuildingEntityAsync(dto.Id);
                entity.ModifyDate = DateTime.Now;
                var Details = GetDetailsUpdate(entity, dto);

                entity.Name = dto.Name;
                entity.Description = dto.Description;
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.Update,
                    CreationDate = DateTime.Now,
                    Details = Details,
                    EmployeeId = employeeId,
                    EntityId = entity.Id,
                    Entity = nameof(Building)
                });

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<bool> DeleteFlat(Guid Id, Guid? employeeId = null)
        {
            try
            {
                var entity = await GetFlatEntityAsync(Id);
                db.Flats.Remove(entity);
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.Delete,
                    CreationDate = DateTime.Now,
                    EmployeeId = employeeId,
                    Entity = nameof(Flat),
                    EntityId = entity.Id
                });
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<bool> DeleteStreet(Guid Id, Guid? employeeId = null)
        {
            try
            {
                var entity = await GetStreetEntityAsync(Id);
                db.Streets.Remove(entity);
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.Delete,
                    CreationDate = DateTime.Now,
                    EmployeeId = employeeId,
                    Entity = nameof(Street),
                    EntityId = entity.Id
                });
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<bool> DeleteBuilding(Guid Id, Guid? employeeId = null)
        {
            try
            {
                var entity = await GetBuildingEntityAsync(Id);
                db.Buildings.Remove(entity);
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.Delete,
                    CreationDate = DateTime.Now,
                    EmployeeId = employeeId,
                    Entity = nameof(Building),
                    EntityId = entity.Id
                });
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<bool> Create(CreateStreetDto dto, Guid? EmployeeId)
        {
            try
            {
                var Id = Guid.NewGuid();
                var street = new Street
                {
                    Id = Id,
                    CreationDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    Name = dto.Name,
                    Description = dto.Description
                };
                await db.AddAsync(street);
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.Create,
                    CreationDate = DateTime.Now,
                    Details = null,
                    EmployeeId = EmployeeId,
                    Entity = nameof(Street),
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
        public async Task<bool> Create(CreateBuildingDto dto, Guid? EmployeeId)
        {
            try
            {
                var Id = Guid.NewGuid();
                var street = new Building
                {
                    Id = Id,
                    CreationDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    Name = dto.Name,
                    Description = dto.Description,
                    StreetId = dto.StreetId
                };
                await db.AddAsync(street);
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.Create,
                    CreationDate = DateTime.Now,
                    Details = null,
                    EmployeeId = EmployeeId,
                    Entity = nameof(Building),
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
        public async Task<bool> Create(CreateFlatDto dto, Guid? EmployeeId)
        {
            try
            {
                var Id = Guid.NewGuid();
                var street = new Flat
                {
                    Id = Id,
                    CreationDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    ApartmentNumber = dto.ApartmentNumber,
                    BuildingId = dto.BuildingId,
                    OwnerId = dto.OwnerId
                };
                await db.AddAsync(street);
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.Create,
                    CreationDate = DateTime.Now,
                    Details = null,
                    EmployeeId = EmployeeId,
                    Entity = nameof(Street),
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

        public async Task<bool> AssignFlatToOwner(Guid flatId, Guid ownerId, Guid? employeeId)
        {
            try
            {
                var flat = await  GetFlatEntityAsync(flatId);
                flat.ModifyDate = DateTime.Now;
                flat.OwnerId = ownerId;

                await db.SaveChangesAsync();
                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.AssignFlatToOwner,
                    CreationDate = DateTime.Now,
                    Details = $"Assign flat ({flatId}) to owner ({ownerId}) ",
                    EmployeeId = employeeId,
                    Entity = nameof(Flat),
                    EntityId = flat.Id
                });
                
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> RemoveAssignFlatToOwner(Guid flatId, Guid ownerId, Guid? employeeId)
        {
            try
            {
                var flat = await GetFlatEntityAsync(flatId);
                flat.ModifyDate = DateTime.Now;
                flat.OwnerId = null;

                await db.SaveChangesAsync();
                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.RemoveAssignFlatToOwner,
                    CreationDate = DateTime.Now,
                    Details = $"Remove assign flat ({flatId}) to owner ({ownerId}) ",
                    EmployeeId = employeeId,
                    Entity = nameof(Flat),
                    EntityId = flat.Id
                });

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IList<ViewFlatDto>> ListFlat(Guid buildingId)
        {
            try
            {
                var entityFlats = await db.Flats.Include(flat => flat.Owner).Where(flat => flat.BuildingId == buildingId).ToListAsync();
                var entityBuilding = await GetBuildingEntityAsync(buildingId);
                var entityStreet = await GetStreetEntityAsync(entityBuilding.StreetId);
                return entityFlats.Select(flat => new ViewFlatDto
                    {
                        Id = flat.Id,
                        CreationDate = flat.CreationDate,
                        ModifyDate = flat.ModifyDate,
                        ApartamentNumber = flat.ApartmentNumber,
                        BuildingId = flat.BuildingId,
                        StreetId = entityBuilding.StreetId,
                        OwnerId = flat.OwnerId,
                        OwnerFullname = flat.OwnerId == null
                            ? string.Empty
                            : $"{flat.Owner.LastName} {flat.Owner.FirstName} {flat.Owner.MiddleName}",
                        FullAddress =
                            $"{Constants.BaseWord.Street} {entityStreet.Name} {Constants.BaseWord.Building} {entityBuilding.Name} {Constants.BaseWord.Apartament} {flat.ApartmentNumber}"
                    })
                    .ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<IList<ViewBuildingDto>> ListBuilding(Guid streetId)
        {
            try
            {
                return await db.Buildings
                    .Include(build => build.Flats)
                    .Include(build => build.Street)
                    .Where(build => build.StreetId == streetId)
                    .Select(build => new ViewBuildingDto
                    {
                        Id = build.Id,
                        CreationDate = build.CreationDate,
                        ModifyDate = build.ModifyDate,
                        Description = build.Description,
                        Street = build.Street.Name,
                        Name = build.Name,
                        StreetId = build.StreetId,
                        FlatIds = build.Flats.Select(flat => flat.Id).ToList()
                    })
                    .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<IList<ViewStreetDto>> ListStreet()
        {
            try
            {
                return await db.Streets
                    .Include(street => street.Buildings)
                    .Select(street => new ViewStreetDto
                    {
                        Id = street.Id,
                        Description = street.Description,
                        Name = street.Name,
                        CreationDate = street.CreationDate,
                        ModifyDate = street.ModifyDate,
                        BuildingIds = street.Buildings.Select(x => x.Id).ToList()
                    }).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IList<ViewBuildingDto>> ListBuilding()
        {
            try
            {
                return await db.Buildings
                    .Include(build => build.Flats)
                    .Include(build => build.Street)
                    .Select(build => new ViewBuildingDto
                    {
                        Id = build.Id,
                        CreationDate = build.CreationDate,
                        ModifyDate = build.ModifyDate,
                        Description = build.Description,
                        Street = build.Street.Name,
                        Name = build.Name,
                        StreetId = build.StreetId,
                        FlatIds = build.Flats.Select(flat => flat.Id).ToList()
                    })
                    .OrderBy(x => x.Street)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<IList<ViewFlatDto>> ListFlat()
        {
            try
            {
                var entityFlats = await db.Flats
                    .Include(flat => flat.Owner)
                    .Include(x=>x.Building)
                    .ThenInclude(x => x.Street)
                    .ToListAsync();
                return entityFlats.Select(flat => new ViewFlatDto
                    {
                        Id = flat.Id,
                        CreationDate = flat.CreationDate,
                        ModifyDate = flat.ModifyDate,
                        ApartamentNumber = flat.ApartmentNumber,
                        BuildingId = flat.BuildingId,
                        StreetId = flat.Building.StreetId,
                        OwnerId = flat.OwnerId,
                        OwnerFullname = flat.OwnerId == null
                            ? string.Empty
                            : $"{flat.Owner.LastName} {flat.Owner.FirstName} {flat.Owner.MiddleName}",
                        FullAddress =
                            $"{Constants.BaseWord.Street} {flat.Building.Street.Name} {Constants.BaseWord.Building} {flat.Building.Name} {Constants.BaseWord.Apartament} {flat.ApartmentNumber}"
                    })
                    .OrderBy(x => x.FullAddress)
                    .ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }



        public async Task<ViewBuildingDto> GetBuilding(Guid Id)
        {
            try
            {

                return await db.Buildings
                    .Include(build => build.Flats)
                    .Include(build => build.Street)
                    .Select(build => new ViewBuildingDto
                    {
                        Id = build.Id,
                        CreationDate = build.CreationDate,
                        ModifyDate = build.ModifyDate,
                        Description = build.Description,
                        Name = build.Name,
                        Street = build.Street.Name,
                        StreetId = build.StreetId,
                        FlatIds = build.Flats.Select(flat => flat.Id).ToList()
                    }).FirstOrDefaultAsync(x => x.Id == Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<ViewStreetDto> GetStreet(Guid Id)
        {
            try
            {
                return await db.Streets
                    .Include(street => street.Buildings)
                    .Select(street => new ViewStreetDto
                    {
                        Id = street.Id,
                        Description = street.Description,
                        Name = street.Name,
                        ModifyDate = street.ModifyDate,
                        CreationDate = street.CreationDate,
                        BuildingIds = street.Buildings.Select(building => building.Id).ToList()
                    })
                    .FirstOrDefaultAsync(x => x.Id == Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<ViewFlatDto> GetFlat(Guid Id)
        {
            try
            {
                var entityFlat = await GetFlatEntityAsync(Id);
                var entityBuilding = await GetBuildingEntityAsync(entityFlat.BuildingId);
                var entityStreet = await GetStreetEntityAsync(entityBuilding.StreetId);

                return new ViewFlatDto
                {
                    Id = entityFlat.Id,
                    CreationDate = entityFlat.CreationDate,
                    ModifyDate = entityFlat.ModifyDate,
                    BuildingId = entityFlat.BuildingId,
                    StreetId = entityBuilding.StreetId,
                    ApartamentNumber = entityFlat.ApartmentNumber,
                    OwnerId = entityFlat.OwnerId,
                    OwnerFullname = entityFlat.OwnerId == null ? string.Empty : $"{entityFlat.Owner.LastName} {entityFlat.Owner.FirstName} {entityFlat.Owner.MiddleName}",
                    FullAddress =
                        $"{Constants.BaseWord.Street} {entityStreet.Name} {Constants.BaseWord.Building} {entityBuilding.Name} {Constants.BaseWord.Apartament} {entityFlat.ApartmentNumber}"
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IList<ViewFlatDto>> GetFlatsByOwner(Guid ownerId)
        {
            try
            {
                var flats = await db.Flats
                    .Include(flat => flat.Owner)
                    .Include(flat => flat.Building)
                    .ThenInclude(building => building.Street)
                    .Where(flat =>
                        (flat.OwnerId != null)
                        && (flat.OwnerId == ownerId))
                    .ToListAsync();

                return flats.Select(flat => new ViewFlatDto
                    {
                        Id = flat.Id,
                        CreationDate = flat.CreationDate,
                        ModifyDate = flat.ModifyDate,
                        ApartamentNumber = flat.ApartmentNumber,
                        BuildingId = flat.BuildingId,
                        OwnerId = flat.OwnerId,
                        StreetId = flat.Building.StreetId,
                        OwnerFullname = flat.OwnerId == null ? string.Empty : $"{flat.Owner.LastName} {flat.Owner.FirstName} {flat.Owner.MiddleName}",
                        FullAddress =
                            $"{Constants.BaseWord.Street} {flat.Building.Street.Name} {Constants.BaseWord.Building} {flat.Building.Name} {Constants.BaseWord.Apartament} {flat.ApartmentNumber}"
                    })
                    .ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
