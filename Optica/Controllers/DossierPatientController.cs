using Optica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Optica.Controllers
{
    [Authorize]
    public class DossierPatientController : Controller
    {

        private AppDbContext ctx = new AppDbContext();
        
        [Route("patient/doc/", Name = "_listeDossierPatient")]
        public ActionResult Index()
        {
            return View();
        }

       /* [Route("patient/{id}/doc/add/", Name = "_addDossierPatient")] //id du patient
        public ActionResult Add(int id, DossierPat d)
        {
            var patient = ctx.Patients.Find(id);
            if (patient != null)
            {
                if (Request.HttpMethod == "POST")
                {
                    var oldDossier = ctx.DossierPats.Where(x => x.PatientId == id && x.Statut == false).OrderByDescending(x => x.Id).FirstOrDefault();
                    if (oldDossier != null)
                        oldDossier.Statut = true;
                    var d2 = new DossierPat { Motif = d.Motif.Trim(), Statut = d.Statut, PatientId = id, DateEnreg = DateTime.Now};
                    ctx.DossierPats.Add(d2);
                    ctx.SaveChanges();
                    return RedirectToAction("Index","Patient");
                }
                return View(new DossierPat() { Patient = patient });
            }
            return new HttpNotFoundResult();
        }  */                                                                                                                                                                                                                                                
        
       /* [Route("patient/doc/{id}/", Name = "_updateDossierPatient")] //id du dossier
        public ActionResult Update(int id, DossierPat d)
        {
            var d2 = ctx.DossierPats.Find(id);
            if (d2 != null)
            {
                var x = Request;
                if (Request.HttpMethod == "POST")
                {
                    d2.Statut = d.Statut;
                    d2.Motif = d.Motif;
                    ctx.SaveChanges();
                    return RedirectToAction("Index", "Patient");
                }
                return View("Add", d2);
            }
            return RedirectToAction("Index","Patient");
        }*/       
        
	}
}