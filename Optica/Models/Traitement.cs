using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Optica.Models
{
    public class Traitement
    {
        public int Id { get; set; }
        public float PrixAssur { get; set; }
        public float PrixOrd { get; set; }
        public float MontantPaye { get; set; }
        public bool PayeOrd { get; set; }
        public bool PayeAssur { get; set; }
        public string Resultat { get; set; }
        public string Remarque { get; set; }
        public string Recommandation { get; set; }
        public DateTime DateTrait { get; set; }
        public DateTime DateEnregTrait { get; set; }
        public bool Del { get; set; }

        public int MedecinTraitantId { get; set; }
        public int? MedecinRecommandantId { get; set; }

        [ForeignKey("MedecinTraitantId")]
        public Medecin MedecinTraitant { get; set; }
        [ForeignKey("MedecinRecommandantId")]
        public Medecin MedecinRecommandant { get; set; }
        //public int MedecinId { get; set; }
        //public virtual Medecin Medecin { get; set; }

        /*public int ActeMedicalId { get; set; }
        public virtual ActeMedical ActeMedical { get; set; }*/
        public int? AssuranceId { get; set; }
        public virtual Assurance Assurance { get; set; }
        public int DossierPatId { get; set; }
        public virtual DossierPat DossierPat { get; set; }
        //public virtual ICollection<PayementOrd> PayementOrds { get; set; }
        public virtual ICollection<Payement> Payements { get; set; }
        public virtual ICollection<TraitementActeMedical> ActeMedicaux { get; set; }

    }
}