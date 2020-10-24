using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Optica.Models
{
    public class Assurance
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Champ obligatoire")]
        public string Code { get; set; }
        [Required(ErrorMessage="Champ obligatoire")]
        public string Nom { get; set; }
        public DateTime DateEnreg { get; set; }
        public bool Del { get; set; }

        public virtual ICollection<Traitement> Traitements { get; set; }
    }
}