using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects.Address
{
    public class CreateBuildingDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid StreetId { get; set; }
    }
}
