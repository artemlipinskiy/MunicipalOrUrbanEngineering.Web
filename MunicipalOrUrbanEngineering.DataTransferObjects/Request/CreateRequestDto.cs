using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects.Request
{
    public class CreateRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Guid RequestTypeId { get; set; }
        public Guid RequestorId { get; set; }

    }
}
