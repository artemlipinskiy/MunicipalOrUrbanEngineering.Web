using System;

namespace MunicipalOrUrbanEngineering.Entities
{
    public class AppUser
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string HashPassword { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public DateTime RegistrationDate { get; set; }

    }
}
