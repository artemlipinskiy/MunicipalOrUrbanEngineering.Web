using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects
{
    public class CreateBulletinBoardItemDto
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }

        public Guid CreatorId { get; set; }
        public Guid? ToStreetId { get; set; }
        public Guid? ToBuildingId { get; set; }
        public Guid? ToFlatId { get; set; }
        public Guid? ToOwnerId { get; set; }

        public bool ToAllUsers { get; set; }

        public DateTime ShowStart { get; set; }
        public DateTime ShowEnd { get; set; }
    }
}
