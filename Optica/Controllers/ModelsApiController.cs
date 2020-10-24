using Optica.Models;
using Optica.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Optica.Controllers
{
    [RoutePrefix("api")]
    public class ModelsApiController : ApiController
    {
        private AppDbContext ctx = new AppDbContext();

        private static readonly Expression<Func<ControlePat,InfoControle>> Cpatient = 
            x => new InfoControle
        {
            Id = x.Id,
            Nompatient = x.DossierPat.Patient.Nom + x.DossierPat.Patient.Prenom,
            NomMed = x.Medecin.Nom +" " + x.Medecin.Prenoms,
            date = x.DateControle
        };

        [Route("liste-controle-prevu", Name = "_listeControlePrevus")]
        [HttpGet]
        public IEnumerable<InfoControle> ControlesPrevus()
        {
            /*foreach (var c in ctx.ControlePats.ToList().Select(Cpatient))
            {
                yield return c;
            }*/
            //return ;
            return ctx.ControlePats.Where(x => x.DateEffectuerControle == null).Select(Cpatient);
        }

        [Route("alerte/liste-patient-controle/", Name = "_alerteListePatient")]
        [HttpGet]
        public IEnumerable<CPatient> AlerteListePatient() 
        {
            List<CPatient> listePatient = new List<CPatient>();
            var lc = ctx.ControlePats.Where(x => x.DateEffectuerControle == null).ToList();
            var toDay = DateTime.Today;
            if (lc.Count > 0)
            {
                string _nomPat, _nomMed, _cPat;
                foreach (var c in lc)
                {
                    if ((int)(toDay.Subtract(c.DateControle).TotalDays) == -7 || (int) (toDay.Subtract(c.DateControle).TotalDays) == -1) 
                    {
                        _nomPat = c.DossierPat.Patient.Nom +" " + c.DossierPat.Patient.Prenom;
                        _cPat = c.DossierPat.Patient.Contact;
                        _nomMed = c.Medecin.Nom + " " + c.Medecin.Prenoms;
                        listePatient.Add(
                            new CPatient {
                                DateControle = c.DateControle,
                                NomPatient = _nomPat,
                                NumeroPatient = _cPat,
                                NomMedecin = _nomMed
                            }
                        );
                    }
                    
                }
            }
            return listePatient;
        }

       public class InfoControle
        {
            public int Id { get; set; }
            public string Nompatient { get; set; }
            public string NomMed { get; set; }
            public DateTime date { get; set; }
        }
    }
}
