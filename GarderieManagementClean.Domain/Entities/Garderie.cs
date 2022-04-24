

using System.Collections.Generic;

namespace GarderieManagementClean.Domain.Entities
{
    public class Garderie
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual Address Address { get; set; }

        public virtual List<Group> Groups { get; set; }

        public virtual List<ApplicationUser> ApplicationUsers { get; set; }
    }
}
