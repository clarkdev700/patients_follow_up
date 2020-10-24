using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optica.ViewModels
{
    public class ControlePrevu
    {
        public string NomPatient { get; set; }
        public string NomMedecin { get; set; }
        public DateTime DateControle { get; set; }
        public string Retard { get; set; }
        public string NbRestant { get; set; }
        public string Contact { get; set; }
    }
}