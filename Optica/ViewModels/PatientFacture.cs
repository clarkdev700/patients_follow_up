using Optica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optica.ViewModels
{
    public class PatientFacture
    {
        public string NumRecu { get; set; }
        public string Agence { get; set; }
        public string Caisse { get; set; }
        public DateTime Date { get; set; }
        public List<TraitementActeMedical> ActeMedicaux { get; set; }
        public Patient Patient { get; set; }
        public Assurance Assurance { get; set; }
        public float MontantGlobale { get; set; }
        public float MontantPatient { get; set; }
        public float MontantAssurance { get; set; }
        /*public float Paye { get; set; }
        public string ModeReglement { get; set; }*/
        public Payement Payement { get; set; }
        public float ResteApayer { get; set; }
        public List<Payement> Payements { get; set; }
        public string StatutReglement { get; set; }
    }
}