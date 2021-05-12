using Microsoft.EntityFrameworkCore;
using MunicipalOrUrbanEngineering.DataTransferObjects;
using MunicipalOrUrbanEngineering.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunicipalOrUrbanEngineering.Services
{
    public class StatusService
    {
        private readonly MUEContext db;
        public StatusService(MUEContext db)
        {
            this.db = db;
        }
        private async Task<Status> GetEntity(Guid Id)
        {
            try
            {
                return await db.Statuses.FirstOrDefaultAsync(x => x.Id == Id);
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public async Task<ViewStatus> Get(Guid Id)
        {
            try
            {
                return await db.Statuses.Select(status => new ViewStatus { 
                Id = status.Id,
                Description = status.Description,
                EntityName = status.EntityName,
                Name = status.Name
                }).FirstOrDefaultAsync(x => x.Id == Id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<ViewStatus>> List(string entity)
        {
            try
            {
                return await db.Statuses
                     .Where(status => status.EntityName == entity)
                     .Select(status => new ViewStatus
                     {
                         Id = status.Id,
                         Name = status.Name,
                         Description = status.Description,
                         EntityName = status.EntityName
                     }).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<ViewStatus>> List()
        {
            try
            {
                return await db.Statuses
                     .Select(status => new ViewStatus
                     {
                         Id = status.Id,
                         Name = status.Name,
                         Description = status.Description,
                         EntityName = status.EntityName
                     }).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
