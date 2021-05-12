using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects
{
    public class ViewBulletinBoardItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Recipient { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public Guid CreatorId { get; set; }
        public string CreatorFullName { get; set; }
    }
}
