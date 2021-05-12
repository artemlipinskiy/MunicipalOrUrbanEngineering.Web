using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects.Payment
{
    public class ViewPaymentSheetDto
    {
        public Guid Id { get; set; }


        public Guid StatusId { get; set; }
        public string Status { get; set; }


        public Guid PaymentPeriodId { get; set; }
        public string PaymentPeriod { get; set; }

        public string Flat { get; set; }
        public Guid FlatId { get; set; }

        public string Owner { get; set; }
        
        public string Name { get; set; }

        public string Comment { get; set; }

        public decimal Amount { get; set; }

        public bool EnableDetails { get; set; }
        public bool EnablePayment { get; set; }

    }
}
