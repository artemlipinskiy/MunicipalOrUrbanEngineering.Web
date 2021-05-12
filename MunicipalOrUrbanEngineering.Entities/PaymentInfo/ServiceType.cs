using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.Entities
{
    public class ServiceType
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string UnitName { get; set; }

        public bool IsCounterReadings { get; set; }
    }
}
