using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects.Payment
{
    public class ViewPaymentPeriodDto
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Guid? StatusId { get; set; }
        public string Status { get; set; }

        public bool EnableCollectingReadings { get; set; }
        public bool EnableFormationOfReceipts { get; set; }
        public bool EnablePaymentOfReceipts { get; set; }
        public bool EnableClose { get; set; }
    }
}
