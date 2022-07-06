using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dtos.Request
{
    public class EnfantCreateRequest
    {
        [Required]

        public string Nom { get; set; }

        [Required(ErrorMessage = "DateNaissance is required")]
        public DateTime DateNaissance { get; set; }

        public int? GroupId { get; set; }

        // public string Photo { get; set; } = "www.photo-url.com";    

        //public int? GroupId { get; set; }

        //public List<TutorRelationDTO> Tutors { get; set; }

    }
}
