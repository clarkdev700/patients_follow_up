using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optica.ViewModels
{
    public class JournalTraitement
    {
        public DateTime DateTraitement { get; set; }
        public string Tdate { get; set; }
        public string NomPatient { get; set; }
        public string NumeroFacture { get; set; }
        public string NomMedecin { get; set; }
        public string NomMedecinRecommande { get; set; }
        public string MontantAssurance { get; set; }
        public string MontantPatient { get; set; }
        public List<string> ActeMedicaux { get; set; }
        public string Statut { get; set; }
    }
}