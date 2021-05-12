using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.Entities
{
    public class ServiceBill
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public string Comment { get; set; }
        public decimal Value { get; set; }

        public Guid TariffId { get; set; }
        public Tariff Tariff { get; set; }

        public Guid? StatusId { get; set; }
        public Status Status { get; set; }

        public Guid PaymentSheetId { get; set; }
        public PaymentSheet PaymentSheet { get; set; }
    }
}
