using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects.Payment
{
    public class ViewTariffDto
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public DateTime StartTariff { get; set; }
        public DateTime? EndTariff { get; set; }


        public decimal Value { get; set; }
        public decimal DefaultData { get; set; }

        public Guid ServiceTypeId { get; set; }
        public string ServiceType { get; set; }
        public string UnitName { get; set; }

        public Guid FlatId { get; set; }
        public string Flat { get; set; }
        
    }
}
