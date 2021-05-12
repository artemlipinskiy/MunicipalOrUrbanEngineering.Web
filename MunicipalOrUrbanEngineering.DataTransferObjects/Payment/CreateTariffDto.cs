using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects.Payment
{
    public class CreateTariffDto
    {
        public decimal Value { get; set; }
        public decimal DefaultData { get; set; }
        
        public DateTime StartTariff { get; set; }
        public DateTime? EndTariff { get; set; }

        public Guid ServiceTypeId { get; set; }
        public Guid FlatId { get; set; }
    }
}
