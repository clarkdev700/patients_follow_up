using Optica.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Optica.ViewModels
{
    public class TraitementViewModel
    {
        public Traitement Traitement { get; set; }
        [Required(ErrorMessage="Champ obligatoire")]
        public List<int> ActesMed { get; set; }
        public int MedecinTraitant { get; set; }
        public int? MedecinRecommandant { get; set; }
    }
}