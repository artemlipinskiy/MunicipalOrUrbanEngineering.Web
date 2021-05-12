using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects.Address
{
    public class ViewStreetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<Guid> BuildingIds { get; set; } = new List<Guid>();
        
        
        public DateTime CreationDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}
