using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects.Payment
{
   public class CreateServiceBillDto
    {
        public string Comment { get; set; }
        public decimal Value { get; set; }

        public Guid TariffId { get; set; }

        public Guid PaymentSheetId { get; set; }
      
    }
}
