using Optica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optica.ViewModels
{
    public class AssuranceFacture
    {
        public string NumRecu { get; set; }
        public string Agence { get; set; }
        public string Caisse { get; set; }
        public DateTime Date { get; set; }
        public Patient patient { get; set; }
        public Assurance Assurance { get; set; }
        public List<TraitementActeMedical> ActeMedicaux { get; set; }
        public float Montant { get; set; }
        public float MontantPatient { get; set; }
    }
}