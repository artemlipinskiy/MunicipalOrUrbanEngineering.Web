using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects
{
    public class CreateHistoryDto
    {
        public Guid EntityId { get; set; }
        public DateTime CreationDate { get; set; }

        public Guid? EmployeeId { get; set; }

        public Guid? OwnerId { get; set; }

        public string Entity { get; set; }

        public string Action { get; set; }
        public string Details { get; set; }
    }
}
