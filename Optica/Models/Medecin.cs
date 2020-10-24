using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Optica.Models
{
    public class Medecin
    {
        public int Id { get; set; }
        [Required]
        public string NumMat { get; set; }
        [Required(ErrorMessage="Champ obligatoire")]
        public string Nom { get; set; }
        [Required(ErrorMessage="Champ obligatoire")]
        public string Prenoms { get; set; }
        [MaxLength(1)]
        public string Sexe { get; set; }
        public string Titre { get; set; }
        public DateTime? DateEntree { get; set; }
        public DateTime? DateSortie { get; set; }
        public DateTime DateEnreg { get; set; }
        public string Contact { get; set; }
        public bool Del { get; set; }

        //public  virtual ICollection<Traitement> Traitements { get; set; }
        [InverseProperty("MedecinTraitant")]
        public List<Traitement> TraitementExecute { get; set; }
        [InverseProperty("MedecinRecommandant")]
        public List<Traitement> TraitementRecommande { get; set; }
        public  virtual ICollection<ControlePat> ControlePats { get; set; }
    }
}