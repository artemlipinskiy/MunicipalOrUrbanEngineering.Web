using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects.Payment
{
    public class CreateMetersDataDto
    {
        public decimal Value { get; set; }

        public Guid PaymentPeriodId { get; set; }
        public Guid TariffId { get; set; }
    }
}
