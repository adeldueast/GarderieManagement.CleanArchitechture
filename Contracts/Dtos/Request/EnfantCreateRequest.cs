using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dtos.Request
{
    public class EnfantCreateRequest
    {
        public string Nom { get; set; }

        public DateTime DateNaissance { get; set; }

        public string Photo { get; set; } = "www.photo-url.com";

        public int? GroupId { get; set; }

        public List<TutorRelation> Tutors { get; set; }

    }
}
