

using System.Collections.Generic;

namespace GarderieManagementClean.Domain.Entities
{
    public class Garderie
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Address Address { get; set; }

        public List<Group> Groups { get; set; }

    }
}
