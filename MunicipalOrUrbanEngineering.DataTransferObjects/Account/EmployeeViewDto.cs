using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects.Account
{
    public class EmployeeViewDto
    {
        public Guid Id { get; set; }
        public Guid? AppUserId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        
        public string Fullname { get; set; }
    }
}
