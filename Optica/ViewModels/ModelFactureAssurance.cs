using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optica.ViewModels
{
    public class ModelFactureAssurance
    {
        public int Id { get; set; }
        public string DateTraitement { get; set; }
        public DateTime Date { get; set; }
        public string NomAssurance { get; set; }
        public string IdentitePatient { get; set; }
        public string RefFacture { get; set; }
        public float Charge { get; set; }
        public float ResteApayer { get; set; }
    }
}