using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects.Request
{
    public class CreateResponseDto
    {

        public Guid ResponserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Guid ServiceRequestId { get; set; }
    }
}
