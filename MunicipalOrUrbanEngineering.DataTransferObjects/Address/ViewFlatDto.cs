using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects.Address
{
    public class ViewFlatDto
    {
        public Guid Id { get; set; }
        public Guid StreetId { get; set; }
        public Guid BuildingId { get; set; }
        public Guid? OwnerId { get; set; }

        public string FullAddress { get; set; }
        public string ApartamentNumber { get; set; }
        public string OwnerFullname { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}
