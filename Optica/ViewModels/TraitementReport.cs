using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optica.ViewModels
{
    public class TraitementReport
    {
        public string DateDebut { get; set; }
        public string DateFin { get; set; }
        public List<JournalTraitement> Traitements { get; set; }
        public string MontantAssurance { get; set; }
        public string MontantPatient { get; set; }
    }
}