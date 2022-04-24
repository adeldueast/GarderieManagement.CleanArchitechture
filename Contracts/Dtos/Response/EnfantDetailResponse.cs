using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dtos.Response
{
    public class EnfantDetailResponse
    {
        public int Id { get; set; }

        public string Nom { get; set; }

        public DateTime DateNaissance { get; set; }

        public string Photo { get; set; } = "www.photo-url.com";

        public GroupDTO Group { get; set; }

        public List<TutorEnfantDTO> Tutors { get; set; } = new List<TutorEnfantDTO>();
    }
}
