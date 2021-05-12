using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects.Payment
{
    public class ViewServiceBillDto
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public string Comment { get; set; }
        public decimal Value { get; set; }

        public Guid TariffId { get; set; }

        public string ServiceType { get; set; }
        public string UnitName { get; set; }
        public decimal Consume { get; set; }
        public decimal ValueUnit { get; set; }
        public string HasCounter { get; set; }

        public Guid StatusId { get; set; }
        public string Status { get; set; }

        public Guid PaymentSheetId { get; set; }
        public string PaymentSheet { get; set; }

        public bool EnablePayment { get; set; }
    }
}
