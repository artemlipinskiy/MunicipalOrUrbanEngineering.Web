using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.DataTransferObjects.Account
{
    public class AccountRegisterDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public Guid RoleId { get; set; }
    }
}
