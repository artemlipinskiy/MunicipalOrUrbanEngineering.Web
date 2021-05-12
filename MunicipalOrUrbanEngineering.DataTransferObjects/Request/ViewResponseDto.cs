using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects.Request
{
    public class ViewResponseDto
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public Guid ResponserId { get; set; }
        public string Responser { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public Guid ServiceRequestId { get; set; }
    }
}
