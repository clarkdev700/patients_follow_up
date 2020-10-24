using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Optica.Models
{
    public class Patient
    {
        public int Id { get; set; }
        [Required]
        public string NumMat { get; set; }
        [Required(ErrorMessage="Champ obligatoire")]
        public string Nom { get; set; }
        [Required(ErrorMessage="Champ obligatoire")]
        public string Prenom { get; set; }
        [MaxLength(1)]
        public string Sexe { get; set; }
        public Titre Titre { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Adresse { get; set; }
        public string LieuNaissance { get; set; }
        public DateTime? DateNaissance { get; set; }
        public string Profession { get; set; }
        public DateTime DateEnreg { get; set; }
        public bool Del { get; set; }

        public virtual ICollection<DossierPat> Patients { get; set; }
    }

    public enum Titre 
    {
        M,
        Mlle,
        Mme
    } 
}