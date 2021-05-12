using System;
using System.Collections.Generic;
using System.Text;

namespace MunicipalOrUrbanEngineering.Entities
{
    public class Owner
    {
        public Guid Id { get; set; }
        public Guid? AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        public List<Flat> Flats { get; set; } = new List<Flat>();
    }
}
