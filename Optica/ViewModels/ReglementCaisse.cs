using Optica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optica.ViewModels
{
    public class ReglementCaisse
    {
        public string Montant { get; set; }
        public string ModeReglement { get; set; }
        public DateTime Date { get; set; }
        public string DateT { get; set; }
        public string NumFacture { get; set; }
        public Patient patient { get; set; }
        public string NomPatient { get; set; }
        public string SourceReglement { get; set; }
    }
}