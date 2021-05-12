using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects.Payment
{
    public class CreateServiceTypeDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string UnitName { get; set; }

        public bool IsCounterReadings { get; set; }
    }
}
