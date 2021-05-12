using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.Entities
{
    public class History
    {
        public Guid Id { get; set; }
        public Guid EntityId { get; set; }
        public DateTime CreationDate { get; set; }

        public Guid? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public Guid? OwnerId { get; set; }
        public Owner Owner { get; set; }

        public string Entity { get; set; }

        public string Action { get; set; }
        public string Details { get; set; }
    }
}
