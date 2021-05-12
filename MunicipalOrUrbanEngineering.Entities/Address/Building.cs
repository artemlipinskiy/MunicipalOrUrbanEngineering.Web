using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.Entities
{
    public class Building
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid StreetId { get; set; }
        public Street Street { get; set; }

        public List<Flat> Flats { get; set; } = new List<Flat>();
    }
}
