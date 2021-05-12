using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.Entities
{
    public class PaymentSheet
    {
        public Guid Id { get; set; }


        public Guid StatusId { get; set; }
        public Status Status { get; set; }


        public Guid PaymentPeriodId { get; set; }
        public PaymentPeriod PaymentPeriod { get; set; }

        public Flat Flat { get; set; }
        public Guid? FlatId { get; set; }

        public string Name { get; set; }

        public string Comment { get; set; }
        
        public decimal Amount { get; set; }

        public List<ServiceBill> ServiceBills { get; set; } = new List<ServiceBill>();
    }
}
