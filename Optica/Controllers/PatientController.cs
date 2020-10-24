using Optica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Optica.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        private AppDbContext ctx = new AppDbContext();
        private DTraitement dt = new DTraitement();
        
        [Route("patient/index", Name="_listePatient")]
        public ActionResult Index(string q)
        {
            List<Patient> liste;
            if (!string.IsNullOrEmpty(q)) {
                q = q.Trim().ToUpper();
                 liste = ctx.Patients.Where(
               x => (x.Nom.StartsWith(q) || x.Nom == q || x.Nom.Contains(q)) ||
                   (x.Prenom.ToUpper() == q || x.Prenom.ToUpper().Contains(q)) ||
                   (x.NumMat == q || x.NumMat.StartsWith(q) || x.NumMat.Contains(q)) && x.Del == false
               ).OrderBy(x => x.Nom).Take(25).ToList();
            }
            else
            {
                liste = ctx.Patients.Where(x => x.Del == false).OrderByDescending(x => x.Id).Take(25).ToList();
            }
           
            //ViewBag.ListePatients = liste;
            return View(liste);
        }

        [Route("patient/add/", Name="_addPatient")]
        public ActionResult Add(Patient p)
        {

            if (Request.HttpMethod == "POST")
            {
                Titre _titre = Request.Form.Count >= 2 && Request.Form[1] != "" ? (Titre)Enum.Parse(typeof(Titre), Request.Form[1]) : Titre.M;
                //action à faire
                //if (ModelState.IsValid)
                //{
                    var p2 = new Patient
                    {
                        Nom = p.Nom.Trim().ToUpper(),
                        Prenom = dt.Title(p.Prenom.Trim()),
                        Contact = p.Contact,
                        Sexe = p.Sexe,
                        LieuNaissance = p.LieuNaissance,
                        Adresse = p.Adresse,
                        //DateNaissance = p.DateNaissance.Value,
                        Email = p.Email,
                        Profession = p.Profession,
                        DateEnreg = DateTime.Now, 
                        Titre = _titre
                    };
                    if (p.DateNaissance != null)
                        p2.DateNaissance = p.DateNaissance.Value;
                    string matricule = dt.getNumeroMatricule(p2.Nom, p2.Prenom);
                    while (ctx.Patients.Where(x => x.NumMat == matricule && x.Del == false).FirstOrDefault() != null)
                    {
                        matricule = dt.getNumeroMatricule(p2.Nom, p2.Prenom);
                    }
                    p2.NumMat = matricule;
                    ctx.Patients.Add(p2);
                    ctx.SaveChanges();
                    //dossier patient
                    var dossier = new DossierPat() {  NumeroDossier = matricule, DateEnreg = DateTime.Now, PatientId = p2.Id };
                    ctx.DossierPats.Add(dossier);
                    ctx.SaveChanges();
                    return RedirectToAction("indexPatient");
                //}
            }
            List<TitreValue> listeTitre = new List<TitreValue> { new TitreValue { Id = 0, Value = "M" }, new TitreValue { Id = 1, Value = "Mlle" }, new TitreValue { Id = 2, Value = "Mme" } };
            ViewBag.Titres = new SelectList(listeTitre, "Id", "Value");
            return View(new Patient());
        }

        [Route("patient/{id}/", Name="_updatePatient")]
        public ActionResult Update(int id, Patient p)
        {
            var p2 = ctx.Patients.Find(id);
            if (p2 != null)
            {
                var _titre = Request.Form.Count >= 2 && Request.Form[1] != "" ? (Titre)Enum.Parse(typeof(Titre), Request.Form[1]) : Titre.M;
                if (Request.HttpMethod == "POST")
                {
                    p2.Nom = p.Nom.Trim().ToUpper();
                    p2.Prenom = dt.Title(p.Prenom.Trim());
                    p2.Sexe = p.Sexe;
                    p2.Contact = p.Contact;
                    p2.Adresse = p.Adresse;
                    p2.Email = p.Email;
                    p2.LieuNaissance = p.LieuNaissance;
                    p2.Profession = p.Profession;
                    p2.Titre = _titre;
                    //p2.NumMat = p.NumMat; A revoir
                    if (p.DateNaissance != null)
                        p2.DateNaissance = p.DateNaissance.Value;
                    ctx.SaveChanges();
                    return RedirectToAction("indexPatient");
                }
                List<TitreValue> listeTitre = new List<TitreValue>{new TitreValue{ Id = 0 , Value = "M"},new TitreValue{ Id =1 , Value = "Mlle"},new TitreValue{ Id = 2, Value = "Mme"}};
                int _t = (int)p2.Titre;
                ViewBag.Titres = new SelectList(listeTitre, "Id","Value", _t);
                return View("Add", p2);
            }

            return RedirectToAction("Add");
        }

        [Route("patient/{id}/details/", Name="_detailsPatient")]
        public ActionResult details(int id)
        {
            var patient = ctx.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }

            return View(patient);
        }

        [Route("patient/{id}/delete/", Name="_deletePatient")] //id du patient
        public ActionResult delete(int id) {
            var patient = ctx.Patients.Find(id);
            if (patient == null) {
                return HttpNotFound();
            }
            if (Request.HttpMethod == "POST")
            {
                patient.Del = true;
                ctx.SaveChanges();
                return RedirectToAction("indexPatient");
            }
            return View(patient);
        }

        [Route("patient/info/", Name = "_infoPatient")]
        public ActionResult InfoPatient(Patient p)
        {
            /*Patient p = new Patient();
            if (!string.IsNullOrEmpty(orgine))
            {

            }
            else
            {
                p = ctx.Patients.Find(id);
            }*/
            return PartialView(p);
        }

        [Route("patient/", Name="__patientIndex")]
        public ActionResult indexPatient()
        {
            return View("Index2");
        }

        [Route("patient/liste-patient/", Name="__listePatient")]
        public JsonResult ListePatient()
        {
            var _listeP = ctx.Patients.Where(x => x.Del == false).OrderByDescending(x => x.Id).Select(x => new { x.Adresse, x.Contact, x.Nom, x.NumMat, x.Prenom, x.Id, x.Email }).ToList();
            return Json(_listeP, JsonRequestBehavior.AllowGet);
        }
	}

    public struct TitreValue
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}