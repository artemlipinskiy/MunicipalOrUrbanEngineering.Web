using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects.Payment
{
    public class ViewMetersDataDto
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public decimal Value { get; set; }

        public Guid PaymentPeriodId { get; set; }
        public string PaymentPeriod { get; set; }

        public string Address { get; set; }
        public string ServiceType { get; set; }
        public Guid TariffId { get; set; }
    }
}
