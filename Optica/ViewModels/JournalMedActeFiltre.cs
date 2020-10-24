using Optica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optica.ViewModels
{
    public class JournalMedActeFiltre
    {
        public string NomPatient { get; set; }
        public string NomMedecinTraitant { get; set; }
        public string NomMedecinRecommandant { get; set; }
        public Traitement Traitements { get; set; }
    }
}