using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dtos.Request
{
    public class EnfantUpdateRequest
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        public DateTime DateNaissance { get; set; }

        public string Photo { get; set; } = "www.photo-url.com";

        public int? GroupId { get; set; }

        public List<TutorRelationDTO> Tutors { get; set; }
    }
}
