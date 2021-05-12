using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.Entities
{
    public class MetersData
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public decimal Value { get; set; }

        public Guid PaymentPeriodId { get; set; }
        public PaymentPeriod PaymentPeriod { get; set; }

        public Tariff Tariff { get; set; }
        public Guid TariffId { get; set; }

    }
}
