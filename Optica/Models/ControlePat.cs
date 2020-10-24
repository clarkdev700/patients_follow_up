using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optica.Models
{
    public class ControlePat
    {
        public int Id { get; set; }
        public DateTime DateControle { get; set; }
        public DateTime? DateEffectuerControle { get; set; }
        public DateTime DateEnreg { get; set; }

        public int MedecinId { get; set; }
        public virtual Medecin Medecin { get; set; }
        public int DossierPatId { get; set; }
        public virtual DossierPat DossierPat { get; set; }
    }
}