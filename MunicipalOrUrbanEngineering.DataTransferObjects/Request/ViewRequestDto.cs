using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects.Request
{
    public class ViewRequestDto
    {
        public Guid Id { get; set; }
        public string CreationDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public Guid RequestTypeId { get; set; }
        public string RequestType { get; set; }

        public Guid RequestorId { get; set; }
        public string Requestor { get; set; }

        public Guid? StatusId { get; set; }
        public string Status { get; set; }

        public Guid? ResponserId { get; set; }
        public string Responser { get; set; }

        public bool EnableCancel { get; set; }
        public bool EnableResponse { get; set; }
        public bool EnableGetResponse { get; set; }
    }
}
