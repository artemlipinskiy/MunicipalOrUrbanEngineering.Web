using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects.Payment
{
    public class CreatePaymentSheetDto
    {
        public Guid PaymentPeriodId { get; set; }
        public Guid? FlatId { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }

    }
}
