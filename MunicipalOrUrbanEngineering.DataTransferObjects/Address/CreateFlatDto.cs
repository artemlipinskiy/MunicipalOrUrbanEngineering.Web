using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects.Address
{
    public class CreateFlatDto
    {
        public Guid Id { get; set; }
     
        public string ApartmentNumber { get; set; }

        public Guid? OwnerId { get; set; }
        public Guid BuildingId { get; set; }

    }
}
