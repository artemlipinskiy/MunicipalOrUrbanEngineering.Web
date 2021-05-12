using Microsoft.EntityFrameworkCore;
using MunicipalOrUrbanEngineering.DataTransferObjects;
using MunicipalOrUrbanEngineering.DataTransferObjects.Request;
using MunicipalOrUrbanEngineering.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunicipalOrUrbanEngineering.Services
{
    public class RequestService
    {
        private readonly MUEContext db;
        private readonly HistoryService historyService;
        public RequestService(MUEContext db)
        {
            this.db = db;
            historyService = new HistoryService(db);
        }

        private async Task<ServiceRequest> GetEntityRequest(Guid Id)
        {
            try
            {
                return await db.ServiceRequests.FirstOrDefaultAsync(x => x.Id == Id);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task<ServiceResponse> GetEntityResponse(Guid Id)
        {
            try
            {
                return await db.ServiceResponses.FirstOrDefaultAsync(x => x.Id == Id);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task<RequestType> GetEntityType(Guid Id)
        {
            try
            {
                return await db.RequestTypes.FirstOrDefaultAsync(x => x.Id == Id);
            }
            catch (Exception)
            {

                throw;
            }
        }
    
        public async Task<bool> DeleteRequest(Guid Id, Guid? ownerId, Guid? employeeId)
        {
            try
            {
                var entity = await GetEntityRequest(Id);
                db.ServiceRequests.Remove(entity);
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto {
                Action = Constants.HistoryAction.Delete,
                CreationDate = DateTime.Now,
                Details = string.Empty,
                EmployeeId = employeeId,
                Entity = nameof(ServiceRequest),
                EntityId = entity.Id,
                OwnerId = ownerId
                });
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> DeleteResponse(Guid Id, Guid? ownerId, Guid? employeeId)
        {
            try
            {
                var entity = await GetEntityResponse(Id);
                db.ServiceResponses.Remove(entity);
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.Delete,
                    CreationDate = DateTime.Now,
                    Details = string.Empty,
                    EmployeeId = employeeId,
                    Entity = nameof(ServiceResponse),
                    EntityId = entity.Id,
                    OwnerId = ownerId
                });
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> DeleteType(Guid Id, Guid? ownerId, Guid? employeeId)
        {
            try
            {
                var entity = await GetEntityType(Id);
                db.RequestTypes.Remove(entity);
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto
                {
                    Action = Constants.HistoryAction.Delete,
                    CreationDate = DateTime.Now,
                    Details = string.Empty,
                    EmployeeId = employeeId,
                    Entity = nameof(RequestType),
                    EntityId = entity.Id,
                    OwnerId = ownerId
                });
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Create(CreateRequestTypeDto dto, Guid? employeeId)
        {
            try
            {
                Guid Id = Guid.NewGuid();
                var entity = new RequestType
                {
                    Id = Id,
                    Description = dto.Description,
                    Name = dto.Description
                };
                await db.RequestTypes.AddAsync(entity);
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto { 
                Action = Constants.HistoryAction.Create,
                CreationDate = DateTime.Now,
                Details = string.Empty,
                EmployeeId = employeeId,
                Entity = nameof(RequestType),
                EntityId = Id,
                OwnerId = null
                });
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> Create(CreateRequestDto dto, Guid ownerId)
        {
            try
            {
                Guid Id = Guid.NewGuid();
                var entity = new ServiceRequest
                {
                    Id = Id,
                    CreationDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    Description = dto.Description,
                    Name = dto.Name,
                    RequestorId = dto.RequestorId,
                    RequestTypeId = dto.RequestTypeId,
                    StatusId = Constants.RequestStatus.New
                };
                await db.AddAsync(entity);
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto {
                Action = Constants.HistoryAction.Create,
                CreationDate = DateTime.Now,
                Details = string.Empty,
                EmployeeId = null,
                Entity = nameof(ServiceRequest),
                EntityId = Id,
                OwnerId = ownerId
                });
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> Create(CreateResponseDto dto, Guid? employeeId)
        {
            try
            {
                Guid id = Guid.NewGuid();
                var entity = new ServiceResponse
                {
                    Id = id,
                    CreationDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    Description = dto.Description,
                    Name = dto.Name,
                    ServiceRequestId = dto.ServiceRequestId,
                    ResponserId = dto.ResponserId
                };
                await db.ServiceResponses.AddAsync(entity);
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto{
                Action = Constants.HistoryAction.Create,
                CreationDate = DateTime.Now,
                Details = $"Response for request {dto.ServiceRequestId}",
                EmployeeId = employeeId,
                Entity = nameof(ServiceResponse),
                EntityId = id,
                OwnerId = null
                });
                return true;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> SetStatus(Guid requestId, Guid statusId, Guid? ownerId, Guid? employeeId) 
        {
            try
            {
                var entity = await GetEntityRequest(requestId);

                Guid oldStatus = (Guid)entity.StatusId;
                entity.StatusId = statusId;
                entity.ModifyDate = DateTime.Now;
                await db.SaveChangesAsync();

                await historyService.Create(new CreateHistoryDto {
                    Action = Constants.HistoryAction.SetStatus,
                    CreationDate = DateTime.Now,
                    Details = $"Change status from {oldStatus} to {statusId}",
                    Entity = nameof(ServiceRequest),
                    EntityId = entity.Id,
                    EmployeeId = employeeId,
                    OwnerId = ownerId
                });
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ViewRequestDto> GetRequest(Guid Id)
        {
            try
            {
                var request = await db.ServiceRequests
                    .Include(x => x.Requestor)
                    .Include(x => x.Status)
                    .Include(x => x.Responser)
                    .FirstOrDefaultAsync(r => r.Id == Id);
                return new ViewRequestDto
                {
                    Id = request.Id,
                    CreationDate = $"{request.CreationDate.Day}.{request.CreationDate.Month}.{request.CreationDate.Year}",
                    Description = request.Description,
                    ModifyDate = request.ModifyDate,
                    Name = request.Name,
                    Status = request.Status.Name,
                    StatusId = request.StatusId,
                    Requestor = $"{request.Requestor.LastName} {request.Requestor.FirstName} {request.Requestor.MiddleName}",
                    RequestorId = request.RequestorId,
                    RequestType = request.RequestType.Name,
                    RequestTypeId = request.RequestTypeId,
                    ResponserId = request.ResponserId,
                    Responser = request.ResponserId == null ? string.Empty : $"{request.Responser.LastName} {request.Responser.FirstName} {request.Responser.MiddleName}",
                    EnableCancel = request.StatusId.Value == Constants.RequestStatus.New,
                    EnableResponse = request.StatusId.Value != Constants.RequestStatus.Cancel && request.StatusId.Value != Constants.RequestStatus.Completed,
                    EnableGetResponse = request.StatusId.Value == Constants.RequestStatus.Completed

                };
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ViewResponseDto> GetResponse(Guid Id)
        {
            try
            {
                var response = await db.ServiceResponses
                    .Include(x => x.Responser)
                    .Include(x => x.ServiceRequest)
                    .FirstOrDefaultAsync(x => x.Id == Id);
                return new ViewResponseDto { 
                Id = response.Id,
                CreationDate = response.CreationDate,
                ModifyDate = response.ModifyDate,
                Description = response.Description,
                Name = response.Name,
                ResponserId = response.ResponserId,
                Responser = $"{response.Responser.LastName} {response.Responser.FirstName} {response.Responser.MiddleName}",
                ServiceRequestId = response.ServiceRequestId
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ViewResponseDto> GetResponseForRequest(Guid requestId)
        {
            try
            {
                var response = await db.ServiceResponses
                    .Include(x => x.Responser)
                    .Include(x => x.ServiceRequest)
                    .FirstOrDefaultAsync(x => x.ServiceRequestId == requestId);
                return new ViewResponseDto
                {
                    Id = response.Id,
                    CreationDate = response.CreationDate,
                    ModifyDate = response.ModifyDate,
                    Description = response.Description,
                    Name = response.Name,
                    ResponserId = response.ResponserId,
                    Responser = $"{response.Responser.LastName} {response.Responser.FirstName} {response.Responser.MiddleName}",
                    ServiceRequestId = response.ServiceRequestId
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IList<ViewRequestTypeDto>> ListType()
        {
            try
            {
                return await db.RequestTypes.Select(type => new ViewRequestTypeDto
                {
                    Id = type.Id,
                    Description = type.Description,
                    Name = type.Name
                }).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IList<ViewRequestDto>> ListRequest()
        {
            try
            {
                var requests = await db.ServiceRequests
                    .Include(x => x.Requestor)
                    .Include(x => x.Status)
                    .Include(x => x.Responser)
                    .Include(x => x.RequestType)
                    .OrderByDescending(r => r.CreationDate).ToListAsync();
                return requests.Select(r => new ViewRequestDto
                {
                    Id = r.Id,
                    CreationDate =$"{r.CreationDate.Date}.{r.CreationDate.Month}.{r.CreationDate.Year}",
                    Description = r.Description,
                    ModifyDate = r.ModifyDate,
                    Name = r.Name,
                    Status = r.Status.Name,
                    StatusId = r.StatusId,
                    Requestor = $"{r.Requestor.LastName} {r.Requestor.FirstName} {r.Requestor.MiddleName}",
                    RequestorId = r.RequestorId,
                    RequestType = r.RequestType.Name,
                    RequestTypeId = r.RequestTypeId,
                    ResponserId = r.ResponserId,
                    Responser = r.ResponserId == null ? string.Empty : $"{r.Responser.LastName} {r.Responser.FirstName} {r.Responser.MiddleName}",
                    EnableCancel = r.StatusId.Value == Constants.RequestStatus.New,
                    EnableResponse = r.StatusId.Value != Constants.RequestStatus.Cancel && r.StatusId.Value != Constants.RequestStatus.Completed,
                    EnableGetResponse = r.StatusId.Value == Constants.RequestStatus.Completed
                }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IList<ViewRequestDto>> ListRequest(Guid requestorId)
        {
            try
            {
                var requests = await db.ServiceRequests
                    .Include(x => x.Requestor)
                    .Include(x => x.Status)
                    .Include(x => x.Responser)
                    .Include(x => x.RequestType)
                    .Where(r => r.RequestorId == requestorId)
                    .OrderByDescending(r => r.CreationDate).ToListAsync();
                return requests.Select(r => new ViewRequestDto
                {
                    Id = r.Id,
                    CreationDate =$"{r.CreationDate.Date}.{r.CreationDate.Month}.{r.CreationDate.Year}",
                    Description = r.Description,
                    ModifyDate = r.ModifyDate,
                    Name = r.Name,
                    Status = r.Status.Name,
                    StatusId = r.StatusId,
                    Requestor = $"{r.Requestor.LastName} {r.Requestor.FirstName} {r.Requestor.MiddleName}",
                    RequestorId = r.RequestorId,
                    RequestType = r.RequestType.Name,
                    RequestTypeId = r.RequestTypeId,
                    ResponserId = r.ResponserId,
                    Responser = r.ResponserId == null ? string.Empty : $"{r.Responser.LastName} {r.Responser.FirstName} {r.Responser.MiddleName}",
                    EnableCancel = r.StatusId.Value == Constants.RequestStatus.New,
                    EnableResponse = r.StatusId.Value != Constants.RequestStatus.Cancel && r.StatusId.Value != Constants.RequestStatus.Completed,
                    EnableGetResponse = r.StatusId.Value == Constants.RequestStatus.Completed
                }).ToList();
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<IList<ViewResponseDto>> ListResponse() 
        {
            try
            {
                var responses = await db.ServiceResponses
                    .Include(x => x.Responser)
                    .Include(x => x.ServiceRequest)
                    .ToListAsync();
                return responses.Select(response => new ViewResponseDto
                {
                    Id = response.Id,
                    CreationDate = response.CreationDate,
                    ModifyDate = response.ModifyDate,
                    Description = response.Description,
                    Name = response.Name,
                    ResponserId = response.ResponserId,
                    Responser = $"{response.Responser.LastName} {response.Responser.FirstName} {response.Responser.MiddleName}",
                    ServiceRequestId = response.ServiceRequestId
                }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IList<ViewResponseDto>> ListResponse(Guid responserId)
        {
            try
            {
                var responses = await db.ServiceResponses
                    .Include(x => x.Responser)
                    .Include(x => x.ServiceRequest)
                    .Where(x => x.ResponserId == responserId)
                    .ToListAsync();
                return responses.Select(response => new ViewResponseDto
                {
                    Id = response.Id,
                    CreationDate = response.CreationDate,
                    ModifyDate = response.ModifyDate,
                    Description = response.Description,
                    Name = response.Name,
                    ResponserId = response.ResponserId,
                    Responser = $"{response.Responser.LastName} {response.Responser.FirstName} {response.Responser.MiddleName}",
                    ServiceRequestId = response.ServiceRequestId
                }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
