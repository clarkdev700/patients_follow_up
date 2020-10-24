using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optica.Models
{
    public class TraitementActeMedical
    {
        public int Id { get; set; }
        public int TraitementId { get; set; }
        public int ActeMedicalId { get; set; }
        public bool Del { get; set; }

        public virtual Traitement Traitement { get; set; }
        public virtual ActeMedical ActeMedical { get; set; }
    }
}