using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace MunicipalOrUrbanEngineering.Entities
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<AppUser> AppUsers { get; set; } = new List<AppUser>();
    }
}
