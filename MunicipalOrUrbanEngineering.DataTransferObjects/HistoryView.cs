using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects
{
    public class HistoryView
    {
        public Guid Id { get; set; }
        public Guid EntityId { get; set; }
        public DateTime CreationDate { get; set; }

        public Guid? EmployeeId { get; set; }
        public string Employee { get; set; }

        public Guid? OwnerId { get; set; }
        public string Owner { get; set; }

        public string Entity { get; set; }

        public string Action { get; set; }
        public string Details { get; set; }
    }
}
