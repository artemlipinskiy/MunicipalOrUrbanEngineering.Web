using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.Entities
{
    public class BulletinBoardItem
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }

        public Guid CreatorId { get; set; }
        public Employee Creator { get; set; }

        public Guid? ToStreetId { get; set; }
        public Street ToStreet { get; set; }
        public Guid? ToBuildingId { get; set; }
        public Building ToBuilding { get; set; }
        public Guid? ToFlatId { get; set; }
        public Flat ToFlat { get; set; }
        public Guid? ToOwnerId { get; set; }
        public Owner ToOwner { get; set; }

        public bool ToAllUsers { get; set; }

        public DateTime ShowStart { get; set; }
        public DateTime ShowEnd { get; set; }
    }
}
