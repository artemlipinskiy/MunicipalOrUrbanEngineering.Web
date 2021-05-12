using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects.Account
{
    public class UserViewDto
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public RoleDto Role { get; set; }
        public Guid RoleId { get; set; }
        public string FullName { get; set; }
    }
}
