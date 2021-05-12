using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.Entities
{
    public class Tariff
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public DateTime StartTariff { get; set; }
        public DateTime? EndTariff { get; set; }
        
        public decimal Value { get; set; }
        public decimal DefaultData { get; set; }

        public Guid ServiceTypeId { get; set; }
        public ServiceType ServiceType { get; set; }

        public Guid FlatId { get; set; }
        public Flat Flat { get; set; }
    }
}
