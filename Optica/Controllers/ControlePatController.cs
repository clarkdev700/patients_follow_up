using Optica.Models;
using Optica.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Optica.Controllers
{
    [Authorize]
    public class ControlePatController : Controller
    {
        private AppDbContext ctx = new AppDbContext();
        
        [Route("controle-patient/historique/{id}/", Name = "_historiqueControlePatient")] //id du patient
        public ActionResult Index(int id)
        {
            var dossiers = ctx.DossierPats.Where(x => x.PatientId == id).ToList();
            var patient = ctx.Patients.Find(id) ; ///!= null ? dossiers.FirstOrDefault().Patient : new Patient();
            ViewBag.dossiers = dossiers;
            return View(patient);
        }

        [Route("controle-patient/{id}/add", Name="_addControlePatient")] //id du patient
        public ActionResult Add(int id, ControlePat c)
        {
            var patient = ctx.Patients.Find(id);
            if (patient == null)
                return new HttpNotFoundResult();

            if (Request.HttpMethod == "POST")
            {
                var d = ctx.DossierPats.Where(x => x.PatientId == id && x.Statut == false).OrderByDescending(x => x.Id).FirstOrDefault();
                if (d != null) 
                {
                    var c2 = new ControlePat { 
                        DateControle = new DateTime(c.DateControle.Year, c.DateControle.Month, c.DateControle.Day), 
                        DateEnreg = DateTime.Now, 
                        DossierPatId = d.Id , 
                        MedecinId = c.MedecinId
                    };
                    ctx.ControlePats.Add(c2);
                    ctx.SaveChanges();
                    return RedirectToAction("indexPatient", "Patient");
                }
            }
            ViewBag.patient = patient;
            var Med = ctx.Medecins.Where(x => x.Del == false).ToList();
            ViewBag.Medecins = new SelectList(Med, "Id", "Nom");
            return View(new ControlePat());
        }

        [Route("controle-patient/{id}/", Name="_updateControlePatient")] //id du controle prevu
        public ActionResult Update(int id, ControlePat c)
        {
            var c2 = ctx.ControlePats.Find(id);
            if (c2 != null)
            {
                if (Request.HttpMethod == "POST")
                {
                    c2.MedecinId = c.MedecinId;
                    c2.DateControle = new DateTime(c.DateControle.Year, c.DateControle.Month, c.DateControle.Day);
                    if (c.DateEffectuerControle != null)
                        c2.DateEffectuerControle = new DateTime(c.DateEffectuerControle.Value.Year, c.DateEffectuerControle.Value.Month, c.DateEffectuerControle.Value.Day);
                    ctx.SaveChanges();
                    return RedirectToAction("indexPatient", "Patient");
                }
                var Med = ctx.Medecins.Where(x => x.Del == false).ToList();
                ViewBag.Medecins = new SelectList(Med, "Id", "Nom", c2.MedecinId);
                ViewBag.patient = c2.DossierPat.Patient;
                return View("Add",c2);
            }
            return RedirectToAction("Index");
        }

        [Route("controle-patient/liste-controle-prevu", Name="_listedeControlePrevu")]
        public ActionResult ListeControlePrevus()
        {
            List<ControlePrevu> ctrl = new List<ControlePrevu>();
            var lc = ctx.ControlePats.Where(x => x.DateEffectuerControle == null).ToList();
            if (lc.Count > 0)
            {
                foreach (var c in lc)
                {
                    var elt = new ControlePrevu
                    {
                        NomPatient = c.DossierPat.Patient.Nom + " " + c.DossierPat.Patient.Prenom,
                        NomMedecin = c.Medecin.Nom + " " + c.Medecin.Prenoms,
                        DateControle = c.DateControle,
                        Contact = c.DossierPat.Patient.Contact
                    };
                    var nbecoule = (int) DateTime.Today.Subtract(c.DateControle).TotalDays;
                    if (nbecoule > 0)
                    {
                        elt.Retard = nbecoule.ToString();
                        elt.NbRestant = "--";
                    }
                    else
                    {
                        elt.NbRestant = nbecoule.ToString() + "jrs";
                        elt.Retard = "--";
                    }
                    ctrl.Add(elt);
                }
            }
            return View("ControlePatient", ctrl);
        }
	}

    
}