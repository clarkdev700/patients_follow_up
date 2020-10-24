using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optica.Models
{
    public class DossierPat
    {
        public int Id { get; set; }
        public string NumeroDossier { get; set; }
        public bool Statut { get; set; }
        public DateTime DateEnreg { get; set; }
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual ICollection<Traitement> Traitements { get; set; }
        public virtual ICollection<ControlePat> ControlePats { get; set; }
    }
}