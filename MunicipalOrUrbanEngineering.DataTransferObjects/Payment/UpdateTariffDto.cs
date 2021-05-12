using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects.Payment
{
    public class UpdateTariffDto
    {
        public Guid Id { get; set; }

        public DateTime StartTariff { get; set; }
        public DateTime? EndTariff { get; set; }

        public decimal Value { get; set; }
        public decimal DefaultData { get; set; }
    }
}
