using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optica.ViewModels
{
    public class CPatient
    {
        public string NomPatient { get; set; }
        public string NumeroPatient { get; set; }
        public string NomMedecin { get; set; }
        public DateTime DateControle { get; set; }
    }
}