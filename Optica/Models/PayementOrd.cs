using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optica.Models
{
    public class PayementOrd
    {
        public int Id { get; set; }
        public string TypePaye { get; set; }
        public string InfoCheque { get; set; }
        public float MontantPaye { get; set; }
        public DateTime DatePaye { get; set; }
        public DateTime DateEnreg { get; set; }
        public bool Del { get; set; }

        //public int TraitementId { get; set; }
        //public virtual Traitement Traitement { get; set; }
    }
}