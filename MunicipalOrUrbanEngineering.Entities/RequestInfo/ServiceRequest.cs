using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.Entities
{
    public class ServiceRequest
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public Guid RequestTypeId { get; set; }
        public RequestType RequestType { get; set; }

        public Guid RequestorId { get; set; }
        public Owner Requestor { get; set; }

        public Guid? StatusId { get; set; }
        public Status Status { get; set; }

        public Guid? ResponserId { get; set; }
        public Employee Responser { get; set; }
    }
}
