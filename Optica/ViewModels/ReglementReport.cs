using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optica.ViewModels
{
    public class ReglementReport
    {
        public string DateDebut { get; set; }
        public string DateFin { get; set; }
        public List<ReglementCaisse> Rc { get; set; }
        public float MontantReport { get; set; }
    }
}