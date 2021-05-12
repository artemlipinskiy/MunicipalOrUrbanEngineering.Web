using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.Entities
{
    public class Status
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EntityName { get; set; }

        public List<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
    }
}
