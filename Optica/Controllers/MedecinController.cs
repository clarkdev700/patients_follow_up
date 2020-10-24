using Optica.Models;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Optica.Controllers
{
    [Authorize]
    public class MedecinController : Controller
    {
        private AppDbContext ctx = new AppDbContext();
        private DTraitement dt = new DTraitement();

        [Route("medecin/index", Name="_listeMedecin")]
        public ActionResult Index(string q)
        {
            List<Medecin> listeMed = new List<Medecin>();
            
            if (!string.IsNullOrEmpty(q)) 
            {
                q = q.Trim().ToUpper();
                listeMed = ctx.Medecins.Where(x => x.Del == false && 
                    (x.Nom == q ||x.Nom.StartsWith(q))||
                    (x.Prenoms.ToUpper() == q || x.Prenoms.ToUpper().Contains(q)) ||
                    (x.NumMat.ToUpper() == q || x.NumMat.ToUpper().StartsWith(q))
                    ).OrderBy(x=>x.Nom).Take(25).ToList();
            }
            if (listeMed.Count == 0) 
            {
                listeMed = ctx.Medecins.Where(x => x.Del == false).OrderByDescending(x=>x.Id).Take(25).ToList();
            } 
            
            return View(listeMed);
        }

        [Route("medecin/add/", Name="addMedecin")]
        public ActionResult Add(Medecin med)
        {
            if (Request.HttpMethod == "POST")
            {
                //if (ModelState.IsValid)
                //{
                ViewBag.ErrMsg = null;
                    var med2 = new Medecin
                    {
                        Nom = med.Nom.Trim().ToUpper(),
                        Prenoms = dt.Title(med.Prenoms.Trim()),
                        Titre = string.IsNullOrEmpty(med.Titre) ? med.Titre : med.Titre.Trim(),
                        Contact = med.Contact,
                        Sexe = med.Sexe,
                        DateEnreg = DateTime.Now,
                        DateEntree = med.DateEntree.Value.Date
                    };
                    var oldmed = ctx.Medecins.Where(x => x.Nom.ToUpper() == med2.Nom && x.Prenoms.ToUpper() == med2.Prenoms.ToUpper()).FirstOrDefault();
                    if (oldmed == null)
                    {
                        var matricule = dt.getNumeroMatricule(med2.Nom, med2.Prenoms);
                        while (ctx.Medecins.Where(x => x.NumMat == matricule).FirstOrDefault() != null)
                        {
                            matricule = dt.getNumeroMatricule(med2.Nom, med2.Prenoms);
                        }
                        med2.NumMat = matricule;
                        ctx.Medecins.Add(med2);
                        ctx.SaveChanges();
                        return RedirectToAction("IndexMedecin");
                    }
                    else
                    {
                        ViewBag.ErrMsg = "Erreur le medecin "+med2.Nom.ToUpper() + " "+ med2.Prenoms+" existe déja vous ne pouvez plus le créer à nouveau";
                    } 
                //}
                
            }
            return View(new Medecin { DateEntree = DateTime.Now });
        }

        [Route("medecin/{id}/", Name="_updateMedecin")]
        public ActionResult Update(int id, Medecin med)
        {
            var med2 = ctx.Medecins.Find(id);
            if (med2 != null)
            {
                if (Request.HttpMethod == "POST")
                {
                    //med2.NumMat = med.NumMat; A revoir
                    med2.Nom = med.Nom.Trim().ToUpper();
                    med2.Prenoms = dt.Title(med.Prenoms.Trim());
                    med2.Titre = string.IsNullOrEmpty(med.Titre) ? med.Titre : med.Titre.Trim();
                    med2.Sexe = med.Sexe;
                    med2.Contact = med.Contact;
                    med2.DateEntree = med.DateEntree;
                    if (med.DateSortie != null && med.DateSortie.Value.Year != 1)
                        med2.DateSortie = med.DateSortie.Value.Date; //new DateTime(med.DateSortie.Value.Year, med.DateSortie.Value.Month, med.DateSortie.Value.Day);
                    ctx.SaveChanges();
                    return RedirectToAction("IndexMedecin");
                }
                return View("Add", med2);
            }
            return RedirectToAction("Add");
        }

        [Route("medecin/{id}/delete/", Name="_deleteMedecin")]
        public ActionResult delete(int id) {
            var med = ctx.Medecins.Find(id);
            if (med == null)
            {
                return HttpNotFound();
            }
            if (Request.HttpMethod == "POST")
            {
                med.Del = true;
                ctx.SaveChanges();
                return RedirectToAction("IndexMedecin");
            }
            return View(med);
        }

        [Route("medecin/", Name = "_IndexMedoc")]
        public ActionResult IndexMedecin()
        {
            return View("Index2");
        }

        [Route("medecin/liste-medecin", Name="__listeMedecin")]
        public JsonResult ListeMedecin()
        {
            var listeMed = ctx.Medecins.Where(x => x.Del == false).OrderByDescending(x => x.Id).Select(x => new { x.Id, x.Nom, x.Prenoms, x.NumMat, x.Titre, x.Contact}).ToList();
            return Json(listeMed, JsonRequestBehavior.AllowGet);
        }
        
	}
}