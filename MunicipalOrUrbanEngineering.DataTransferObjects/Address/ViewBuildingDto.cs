using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects.Address
{
    public class ViewBuildingDto
    {
        public Guid Id { get; set; }
        public Guid StreetId { get; set; }
        public string Street { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public IList<Guid> FlatIds { get; set; } = new List<Guid>();
    }
}
