using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.Entities
{
    public class Flat
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public string ApartmentNumber { get; set; }


        public Guid? OwnerId { get; set; }
        public Owner Owner { get; set; }
        
        public Building Building { get; set; }
        public Guid BuildingId { get; set; }

        public List<Tariff> Tariffs { get; set; } = new List<Tariff>();
        public List<PaymentSheet> PaymentSheets { get; set; } = new List<PaymentSheet>();
    }
}
