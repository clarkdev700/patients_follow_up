using Optica.Models;
using Optica.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using SelectPdf;
using System.IO;

namespace Optica.Controllers
{
    [Authorize/*(Roles="Admin")*/]
    public class TraitementOrdController : BaseController
    {

        private AppDbContext ctx = new AppDbContext();
        private static TraitementReport _tReport = new TraitementReport(); 

        [Route("patient/{id}/historique-de-traitement/{more?}", Name = "_historiqueTraitement")] //id du patient
        public ActionResult Index(int id, string more)
        {
           
            var patient = ctx.Patients.Find(id) ;
            if (patient == null)
            {
                return new HttpNotFoundResult();
            }
            //trouver les traitements du patients
            IEnumerable<Traitement> traitements;
            if (!string.IsNullOrEmpty(more))
            {
                traitements = from t in ctx.Traitements
                                join dp in ctx.DossierPats on t.DossierPatId equals dp.Id
                                where dp.PatientId == id && t.Del == false
                                orderby t.Id descending
                                select t;
                ViewBag.btnState = "moins";
            }
            else
            {
                traitements = from t in ctx.Traitements
                                join dp in ctx.DossierPats on t.DossierPatId equals dp.Id
                                where dp.PatientId == id && t.Del == false
                                orderby t.Id descending
                                select t;
                traitements = traitements.Take(10);
                ViewBag.btnState = "more";
            }
            ViewBag.patient = patient;
            return View(traitements);
        }

        [Route("patient/fiche-traitement/{id}/add/", Name = "_addFicheTraitement")] //id du patient
        public ActionResult Add(int id, TraitementViewModel t)
        {
            var patient = ctx.Patients.Find(id);
            if (patient == null)
            {
                return new HttpNotFoundResult();
            }
            if (Request.HttpMethod == "POST" && ModelState.IsValid)
            {
                //recuperation du dossier pat ouvert
                var docPat = ctx.DossierPats.Where(x => x.Statut == false && x.PatientId == id).FirstOrDefault();
                if (docPat == null)
                {
                    //creer un dossier patient 1 erreur survenu probablement
                    docPat = new DossierPat() { PatientId = id, DateEnreg = DateTime.Now,  NumeroDossier = patient.NumMat };
                    ctx.DossierPats.Add(docPat);
                    ctx.SaveChanges();
                }

                var medTraitant = Request.Form.Count >= 2 && Request.Form[1] != "" ? ctx.Medecins.Find(int.Parse(Request.Form[1])) : null;
                var medRec = Request.Form.Count >= 3 && Request.Form[2] != "" ? ctx.Medecins.Find(int.Parse(Request.Form[2])) : null;
                var t2 = new Traitement
                {                   
                    //ActeMedicalId = t.ActeMedicalId, A REVOIR
                    AssuranceId = t.Traitement.AssuranceId,
                    DateTrait = t.Traitement.DateTrait.Date,
                    DossierPatId = docPat.Id,
                    MedecinTraitant = medTraitant,
                    MedecinRecommandant = medRec,
                    //MedecinId = t.Traitement.MedecinId, AC
                    PrixAssur = t.Traitement.PrixAssur,
                    PrixOrd = t.Traitement.PrixOrd,
                    Recommandation = t.Traitement.Recommandation,
                    Remarque = t.Traitement.Remarque,
                    DateEnregTrait = DateTime.Now
                };
                t2.PayeAssur = t2.PrixAssur == 0 ? true : false;
                t2.PayeOrd = t2.PrixOrd == 0 ? true : false;
                ctx.Traitements.Add(t2);
                ctx.SaveChanges();
                if (t.ActesMed.Count > 0)
                {
                    foreach (var a in t.ActesMed)
                    {
                        ctx.TraitementActeMedicals.Add(new TraitementActeMedical {  ActeMedicalId = a, TraitementId = t2.Id});
                    }
                    ctx.SaveChanges();
                }
                if (Request.Files.Count > 0)
                {
                    SaveImageScan(Request.Files["Resultat"], t2);
                }
                //generation de facture d'assurance
                /*if (t.Traitement.AssuranceId > 0 && t.Traitement.PrixAssur > 0)
                {
                    return RedirectToAction("generateRecu", "ReglementFacture", new { _url = "assurance_Fact", id = t2.Id, _userLog = User.Identity.Name });
                }*/
                
                return RedirectToAction("Index", new { id = id});
            }
            ViewBag.patient = patient;
            var med = ctx.Medecins.Where(x => x.Del == false).OrderBy(x => x.Nom).ToList();
            var assur = ctx.Assurances.Where(y => y.Del == false).OrderBy(x => x.Nom).ToList();
            List<ActeMedical> actMed = ctx.ActeMedicals.Where(a => a.Del == false).ToList();
            ViewBag.Assur = new SelectList(assur, "Id", "Code");
            ViewBag.Med = new SelectList(med, "Id", "Nom");
            ViewBag.MedRec = new SelectList(med, "Id", "Nom");
            //ViewBag.Act = new SelectList(actMed, "Id", "Code");
            ViewBag.Act = new MultiSelectList(actMed, "Id", "Code", null);
            var traitement = new Traitement { DateTrait = DateTime.Now };
            return View(new TraitementViewModel { Traitement = traitement });
        }

        [Route("patient/fiche-traitement/{id}/", Name = "_updateFicheTraitement")] //id fiche patient
        public ActionResult Update(int id, TraitementViewModel t)
        {
            var t2 = ctx.Traitements.Find(id);
            if (t2 != null)
            {
               
                if (Request.HttpMethod == "POST" && ModelState.IsValid)
                {
                    var medTraitant = Request.Form.Count >= 2 && Request.Form[1] != "" ? ctx.Medecins.Find(int.Parse(Request.Form[1])) : null;
                    var medRec = Request.Form.Count >= 3 && Request.Form[2] != "" ? ctx.Medecins.Find(int.Parse(Request.Form[2])) : null;
                    //t2.ActeMedicalId = t.ActeMedicalId; A REVOIR
                    t2.AssuranceId = t.Traitement.AssuranceId;
                    t2.DateTrait = t.Traitement.DateTrait.Date;
                    //t2.MedecinId = t.Traitement.MedecinId; AC
                    t2.MedecinTraitant = medTraitant;
                    t2.MedecinRecommandant = medRec;
                   //t2.PayeAssur = t.Traitement.PayeAssur;
                    //t2.PayeOrd = t.Traitement.PayeOrd;
                    t2.PrixAssur = t.Traitement.PrixAssur;
                    t2.PrixOrd = t.Traitement.PrixOrd;
                    t2.Recommandation = t.Traitement.Recommandation;
                    t2.Remarque = t.Traitement.Remarque;
                   if (t2.PrixAssur == 0) 
                    { t2.PayeAssur = true; } 
                    else 
                    {
                        var mttpaye = ctx.Payements.Where(x => x.Del == false && x.SourcePaye == Source.Assurance && x.TraitementId == id).ToList().Sum(x => x.MontantPaye);
                        t2.PayeAssur = t2.PrixAssur == mttpaye ? true : false;
                    }

                    if (t2.PrixOrd == 0) { t2.PayeOrd = true; }
                    else
                    {
                        var mttpaye = ctx.Payements.Where(x => x.Del == false && x.SourcePaye == Source.Patient && x.TraitementId == id).ToList().Sum(x => x.MontantPaye);
                        t2.PayeOrd = t2.PrixOrd == mttpaye ? true : false;
                    }
                    //t2.PayeAssur = t2.PrixAssur == 0 ? true : false;
                    //t2.PayeOrd = t2.PrixOrd == 0 ? true : false;
                    ctx.SaveChanges();
                    List<int> OldActeMed = t2.ActeMedicaux.Select(x => x.ActeMedicalId).ToList();
                    List<int> eltToDel = new List<int>();
                    List<int> eltInter = new List<int>();
                    if (!OldActeMed.SequenceEqual(t.ActesMed))
                    {
                        foreach (var o in OldActeMed)
                        {
                            if (!t.ActesMed.Contains(o))
                            {
                                eltToDel.Add(o);
                            }
                        }
                        // element already exist
                        eltInter = OldActeMed.Intersect(t.ActesMed).ToList();
                        //delete elInter in t
                        if (eltInter.Count > 0)
                        {
                            foreach (var i in eltInter)
                            {
                                if (t.ActesMed.Contains(i))
                                {
                                    t.ActesMed.Remove(i);
                                }
                            }
                        }
                        //update data in db
                        if (t.ActesMed.Count > 0)
                        {
                            foreach (var na in t.ActesMed)
                            {
                                ctx.TraitementActeMedicals.Add(new TraitementActeMedical { ActeMedicalId = na, TraitementId = t2.Id });
                            }
                        }
                        if (eltToDel.Count > 0)
                        {
                            foreach (var oa in eltToDel)
                            {
                                var actToDel = ctx.TraitementActeMedicals.Where(x => x.ActeMedicalId == oa && x.TraitementId == t2.Id && x.Del == false).FirstOrDefault();
                                if (actToDel != null)
                                    actToDel.Del = true;
                            }
                        }
                        ctx.SaveChanges();
                        
                    }
                    
                    if (Request.Files.Count > 0)
                    {
                        SaveImageScan(Request.Files["Resultat"], t2);
                    }
                    //generation de facture d'assurance
                    /*if (t.Traitement.AssuranceId > 0 && t.Traitement.PrixAssur > 0)
                    {
                        return RedirectToAction("generateRecu", "ReglementFacture", new { _url = "assurance_Fact", id = t2.Id, _userLog = User.Identity.Name });
                    }*/
                    return RedirectToAction("Index", new { id = t2.DossierPat.PatientId });
                }
                ViewBag.patient = t2.DossierPat.Patient;
                var med = ctx.Medecins.Where(x => x.Del == false).OrderBy(x => x.Nom).ToList();
                var assur = ctx.Assurances.Where(y => y.Del == false).OrderBy(x => x.Nom).ToList();
                var actMed = ctx.ActeMedicals.Where(a => a.Del == false).ToList();
                ViewBag.Assur = new SelectList(assur, "Id", "Code",t2.AssuranceId);
                ViewBag.Med = new SelectList(med, "Id", "Nom", t2.MedecinTraitant.Id);
                string idMedRec = t2.MedecinRecommandant != null ? t2.MedecinRecommandant.Id.ToString(): null;
                ViewBag.MedRec = new SelectList(med, "Id", "Nom", idMedRec);
                ViewBag.Act = new MultiSelectList(actMed, "Id", "Code", t2.ActeMedicaux.Where(x => x.Del == false).Select(x => x.ActeMedicalId).ToList());
                var traitement = new TraitementViewModel { Traitement = t2 };
                return View("Add", traitement);
            }
            return RedirectToAction("Index", "Patient");
        }
        [Route("patient/historique-traitement/{id}/del/", Name="_DelTraitementGet")]
        [HttpGet]
        public ActionResult DelTraitement(int id)
        {
            var traitement = ctx.Traitements.Find(id);
            if (traitement == null)
                return new HttpNotFoundResult();
            return View(traitement);
        }

        [Route("patient/historique-traitement/{id}/del/", Name="_DelTraitement")]
        [HttpPost]
        public ActionResult DeleteTraitement(int id) 
        {
            var traitement = ctx.Traitements.Find(id);
            if (traitement == null)
                return new HttpNotFoundResult();
            traitement.Del = true;
            //delete all acte
            /*if (traitement.ActeMedicaux.ToList().Count > 0)
            {
                foreach (var acte in traitement.ActeMedicaux.ToList())
                {
                    acte.Del = true;
                }
            }*/
            //delete all payements
            if (traitement.Payements.ToList().Count > 0)
            {
                foreach (var p in traitement.Payements.ToList())
                {
                    p.Del = true;
                }
            }
            //save changes
            ctx.SaveChanges();
            return RedirectToRoute("_historiqueTraitement", new { id = traitement.DossierPat.PatientId});
        }

        [Route("administration/traitement-patient/", Name = "_journalTraitementPatient")]
        [HttpGet]
        public ActionResult TraitementJournal()
        {
            ViewBag.deb = DateTime.Now;
            var medecins = ctx.Medecins.Where(x=> x.Del == false).OrderBy(x=> x.Nom).ToList();
            var actes = ctx.ActeMedicals.Where(x => x.Del == false).OrderBy(x => x.Code).ToList();
            ViewBag.Medecins = new MultiSelectList(medecins, "Id", "Nom");
            ViewBag.Actes = new MultiSelectList(actes,"Id","Code");
            return View();
        }

        [Route("administration/journal-traitement/querry", Name = "_journalTraitement")]
        public JsonResult Journal(DateTime deb, DateTime end, string print, int[] Medecins, int[] Actes)
        { 
            List<Traitement> _listeTraitement = new List<Traitement>();
            List<JournalTraitement> _listeJournal = new List<JournalTraitement>();
            List<Traitement> listActe = new List<Traitement>();
            string ddeb, dend;
            //if (deb != null && end != null){ }
            ddeb = deb.ToString("dd/MM/yyyy");
            dend = end.ToString("dd/MM/yyyy");
            if (Medecins!= null && Medecins.Length > 0 && Medecins[0] > 0)
            {          
                foreach(int item in Medecins) 
                {
                    List<Traitement> _actes = GetTraitement(Actes, item, deb, end);
                    List<Traitement> result = listActe.Concat(_actes).ToList();
                    listActe = result;
                }

            }
            else
            {
               listActe = GetTraitement(Actes,0, deb, end);
            }
           // _listeTraitement = ctx.Traitements.Where(x => x.DateTrait >= deb && x.DateTrait <= end).OrderBy(x=> x.DateTrait).ToList();
            
            /*else if (deb != null && end == null)
            {
                ddeb = deb.Value.ToString("dd/MM/yyyy");
                dend = DateTime.Now.ToString("dd/MM/yyyy");
                _listeTraitement = ctx.Traitements.Where(x => x.DateTrait >= deb).OrderBy(x=>x.DateTrait).ToList();
            }
            else if (deb == null && end != null)
            {
                var y = ctx.Patients.OrderBy(x => x.Id).FirstOrDefault();
                ddeb = y != null? y.DateEnreg.ToString("dd/MM/yyyy") : " depuis debut operation";
                dend = end.Value.ToString("dd/MM/yyyy");
                _listeTraitement = ctx.Traitements.Where(x => x.DateTrait <= end).OrderBy(x=>x.DateTrait).ToList();
            }
            else
            {
                var y = ctx.Patients.OrderBy(x => x.Id).FirstOrDefault();
                ddeb = y != null ? y.DateEnreg.ToString("dd/MM/yyyy") : " depuis debut operation";
                dend = DateTime.Now.ToString("dd/MM/yyyy");
                _listeTraitement = ctx.Traitements.OrderBy(x=>x.DateTrait).ToList();
            }*/

            if (listActe.Count > 0)
            {
                foreach (var elt in listActe.OrderBy(x=>x.DateTrait).ToList())
                {
                    var _nomPatient = elt.DossierPat.Patient.Nom +" " +elt.DossierPat.Patient.Prenom;
                    string nomMedTr = "", nomMedRec = "";
                    if (elt.MedecinTraitantId > 0)
                    {
                        var med = ctx.Medecins.Find(elt.MedecinTraitantId);
                        nomMedTr = med != null ? med.Nom +" "+ med.Prenoms:null;
                    }
                    if (elt.MedecinRecommandantId > 0)
                    {
                        var medrec = ctx.Medecins.Find(elt.MedecinRecommandantId);
                        nomMedRec = medrec != null ? medrec.Nom +" "+ medrec.Prenoms:null;
                    }
                    var _statut = elt.Del ? "Annuler" : ""; 
                    var _actArr = elt.ActeMedicaux.Where(x => x.Del == false).Select(x => x.ActeMedical.Code).ToArray();
                    int arlenght = _actArr.Length;
                    List<string> _lacteMed = new List<string>();
                    if (arlenght > 0)
                    {
                        int index = 0;
                        while (index < arlenght)
                        {
                            if (arlenght - index > 1)
                            {
                                _lacteMed.Add(_actArr[index] + " - ");
                            }
                            else
                            {
                                _lacteMed.Add(_actArr[index]);
                            }

                            index++;
                        }
                    }
                    var _numFacture = elt.DateTrait.ToString("dd" + elt.Id + "MM");
                    if (_listeJournal.Count ==0 || (_listeJournal.Count > 0 && _listeJournal.Where(x => x.NumeroFacture == _numFacture).FirstOrDefault() == null))
                    _listeJournal.Add(
                            new JournalTraitement { Tdate = elt.DateTrait.ToString("dd/MM/yyyy"), DateTraitement = elt.DateTrait, NomPatient = _nomPatient, NomMedecin = nomMedTr, NomMedecinRecommande = nomMedRec, MontantAssurance = elt.PrixAssur.ToString("N0", CultureInfo.CurrentCulture), MontantPatient = elt.PrixOrd.ToString("N0", CultureInfo.CurrentCulture), NumeroFacture = _numFacture, ActeMedicaux = _lacteMed, Statut = _statut }
                        );
                }
                if (!string.IsNullOrEmpty(print))
                {
                    //redirection generation du raport
                    string fileState = "ERROR";
                    if (_listeJournal.Count > 0)
                    {
                        var mttAssur = _listeJournal.Where(x=>x.Statut != "Annuler").ToList().Sum(x=> decimal.Parse(x.MontantAssurance));
                        var mttPatient = _listeJournal.Where(x=>x.Statut != "Annuler").ToList().Sum(x => decimal.Parse(x.MontantPatient));
                        _tReport = new TraitementReport { DateDebut = ddeb, DateFin = dend, Traitements = _listeJournal, MontantAssurance = mttAssur.ToString("N0", CultureInfo.CurrentCulture), MontantPatient = mttPatient.ToString("N0", CultureInfo.CurrentCulture) };
                        fileState = generateReport("_reportJournalTraitementFile");
                    }
                    return Json(new { qState = fileState }, JsonRequestBehavior.AllowGet);
                }

            }
            return Json(_listeJournal, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        [Route("administration/Journal-traitement/reportfile/", Name="_reportJournalTraitementFile")]
        public ActionResult TraitementReport()
        {
            return View(_tReport);
        }

        private void SaveImageScan(HttpPostedFileBase formFile, Traitement t) {
            if (formFile.FileName != "")
            {
                var serverPath = Server.MapPath(formFile.FileName);
                var extension = Path.GetExtension(formFile.FileName);
                string fileName = t.Id + extension;
                string saveDir = Server.MapPath(System.IO.Path.Combine("~/Datadir/", fileName));
                if (System.IO.File.Exists(saveDir))
                    System.IO.File.Delete(saveDir);
                formFile.SaveAs(saveDir);
                if (t.Resultat != null)
                {
                    var oldfile = Server.MapPath(Path.Combine("~/DataDir/", t.Resultat));
                    if (System.IO.File.Exists(oldfile))
                        System.IO.File.Delete(oldfile);
                }
                t.Resultat = fileName;
                ctx.SaveChanges();
            }
            
        }

        private List<Traitement> GetTraitement(int[]Actes, int item, DateTime deb, DateTime end)
        {
            List<Traitement> listActe = new List<Traitement>();
            int a1, a2, a3, a4, a5;
            switch (Actes.Length)
            {
                case 1:
                    a1 = Actes[0];
                    if (item > 0 && a1 >0)
                    {
                        listActe = (from a in ctx.ActeMedicals
                                    join tam in ctx.TraitementActeMedicals on a.Id equals tam.ActeMedicalId
                                    join t in ctx.Traitements on tam.TraitementId equals t.Id
                                    join med in ctx.Medecins on t.MedecinTraitantId equals med.Id
                                    where a.Id == a1 && med.Id == item && t.DateTrait >= deb && t.DateTrait <= end 
                                    select t).Union(
                                   from a in ctx.ActeMedicals
                                   join tam in ctx.TraitementActeMedicals on a.Id equals tam.ActeMedicalId
                                   join t in ctx.Traitements on tam.TraitementId equals t.Id
                                   join med in ctx.Medecins on t.MedecinRecommandantId equals med.Id
                                   where a.Id == a1 && med.Id == item && t.DateTrait >= deb && t.DateTrait <= end
                                   select t
                               ).OrderBy(x => x.DateTrait).ToList();
                    }
                    else if(a1 > 0)
                    {
                        listActe = (from a in ctx.ActeMedicals
                                    join tam in ctx.TraitementActeMedicals on a.Id equals tam.ActeMedicalId
                                    join t in ctx.Traitements on tam.TraitementId equals t.Id
                                    where a.Id == a1 && t.DateTrait >= deb && t.DateTrait <= end
                                    select t).Union(
                                    from a in ctx.ActeMedicals
                                    join tam in ctx.TraitementActeMedicals on a.Id equals tam.ActeMedicalId
                                    join t in ctx.Traitements on tam.TraitementId equals t.Id
                                    where a.Id == a1 && t.DateTrait >= deb && t.DateTrait <= end
                                    select t
                                ).OrderBy(x => x.DateTrait).ToList();
                    }
                    else
                    {
                        goto case 0;
                    }

                    break;
                case 2:
                    a1 = Actes[0]; a2 = Actes[1];
                    if (item > 0)
                    {
                        listActe = (from a in ctx.ActeMedicals
                                    join tam in ctx.TraitementActeMedicals on a.Id equals tam.ActeMedicalId
                                    join t in ctx.Traitements on tam.TraitementId equals t.Id
                                    join med in ctx.Medecins on t.MedecinTraitantId equals med.Id
                                    where (a.Id == a1 || a.Id == a2) && med.Id == item && t.DateTrait >= deb && t.DateTrait <= end
                                    select t).Union(
                                    from a in ctx.ActeMedicals
                                    join tam in ctx.TraitementActeMedicals on a.Id equals tam.ActeMedicalId
                                    join t in ctx.Traitements on tam.TraitementId equals t.Id
                                    join med in ctx.Medecins on t.MedecinRecommandantId equals med.Id
                                    where (a.Id == a1 || a.Id == a2) && med.Id == item && t.DateTrait >= deb && t.DateTrait <= end
                                    select t
                                    ).OrderBy(x => x.DateTrait).ToList();
                    }
                    else
                    {
                        listActe = (from a in ctx.ActeMedicals
                                    join tam in ctx.TraitementActeMedicals on a.Id equals tam.ActeMedicalId
                                    join t in ctx.Traitements on tam.TraitementId equals t.Id
                                    where (a.Id == a1 || a.Id == a2) && t.DateTrait >= deb && t.DateTrait <= end
                                    select t).Union(
                                    from a in ctx.ActeMedicals
                                    join tam in ctx.TraitementActeMedicals on a.Id equals tam.ActeMedicalId
                                    join t in ctx.Traitements on tam.TraitementId equals t.Id
                                    where (a.Id == a1 || a.Id == a2) && t.DateTrait >= deb && t.DateTrait <= end
                                    select t
                                    ).OrderBy(x => x.DateTrait).ToList();
                    }

                    break;
                case 3:
                    a1 = Actes[0]; a2 = Actes[1]; a3 = Actes[2];
                    if (item > 0)
                    {
                        listActe = (from a in ctx.ActeMedicals
                                    join tam in ctx.TraitementActeMedicals on a.Id equals tam.ActeMedicalId
                                    join t in ctx.Traitements on tam.TraitementId equals t.Id
                                    join med in ctx.Medecins on t.MedecinTraitantId equals med.Id
                                    where (a.Id == a1 || a.Id == a2 || a.Id == a3) && med.Id == item && t.DateTrait >= deb && t.DateTrait <= end
                                    select t).Union(
                                    from a in ctx.ActeMedicals
                                    join tam in ctx.TraitementActeMedicals on a.Id equals tam.ActeMedicalId
                                    join t in ctx.Traitements on tam.TraitementId equals t.Id
                                    join med in ctx.Medecins on t.MedecinRecommandantId equals med.Id
                                    where (a.Id == a1 || a.Id == a2 || a.Id == a3) && med.Id == item && t.DateTrait >= deb && t.DateTrait <= end
                                    select t
                                    ).OrderBy(x => x.DateTrait).ToList();
                    }
                    else
                    {
                        listActe = (from a in ctx.ActeMedicals
                                    join tam in ctx.TraitementActeMedicals on a.Id equals tam.ActeMedicalId
                                    join t in ctx.Traitements on tam.TraitementId equals t.Id
                                    where (a.Id == a1 || a.Id == a2 || a.Id == a3) && t.DateTrait >= deb && t.DateTrait <= end
                                    select t).Union(
                                    from a in ctx.ActeMedicals
                                    join tam in ctx.TraitementActeMedicals on a.Id equals tam.ActeMedicalId
                                    join t in ctx.Traitements on tam.TraitementId equals t.Id
                                    where (a.Id == a1 || a.Id == a2 || a.Id == a3) && t.DateTrait >= deb && t.DateTrait <= end
                                    select t
                                    ).OrderBy(x => x.DateTrait).ToList();
                    }

                    break;
                case 4:
                    a1 = Actes[0]; a2 = Actes[1]; a3 = Actes[2]; a4 = Actes[3];
                    if (item > 0)
                    {
                        listActe = (from a in ctx.ActeMedicals
                                    join tam in ctx.TraitementActeMedicals on a.Id equals tam.ActeMedicalId
                                    join t in ctx.Traitements on tam.TraitementId equals t.Id
                                    join med in ctx.Medecins on t.MedecinTraitantId equals med.Id
                                    where (a.Id == a1 || a.Id == a2 || a.Id == a3 || a.Id == a4) && med.Id == item && t.DateTrait >= deb && t.DateTrait <= end
                                    select t).Union(
                                    from a in ctx.ActeMedicals
                                    join tam in ctx.TraitementActeMedicals on a.Id equals tam.ActeMedicalId
                                    join t in ctx.Traitements on tam.TraitementId equals t.Id
                                    join med in ctx.Medecins on t.MedecinRecommandantId equals med.Id
                                    where (a.Id == a1 || a.Id == a2 || a.Id == a3 || a.Id == a4) && med.Id == item && t.DateTrait >= deb && t.DateTrait <= end
                                    select t
                                    ).OrderBy(x => x.DateTrait).ToList();
                    }
                    else
                    {
                        listActe = (from a in ctx.ActeMedicals
                                    join tam in ctx.TraitementActeMedicals on a.Id equals tam.ActeMedicalId
                                    join t in ctx.Traitements on tam.TraitementId equals t.Id
                                    where (a.Id == a1 || a.Id == a2 || a.Id == a3 || a.Id == a4) && t.DateTrait >= deb && t.DateTrait <= end
                                    select t).Union(
                                    from a in ctx.ActeMedicals
                                    join tam in ctx.TraitementActeMedicals on a.Id equals tam.ActeMedicalId
                                    join t in ctx.Traitements on tam.TraitementId equals t.Id
                                    where (a.Id == a1 || a.Id == a2 || a.Id == a3 || a.Id == a4) && t.DateTrait >= deb && t.DateTrait <= end
                                    select t
                                    ).OrderBy(x => x.DateTrait).ToList();
                    }

                    break;
                case 5:
                    a1 = Actes[0]; a2 = Actes[1]; a3 = Actes[2]; a4 = Actes[3]; a5 = Actes[4];
                    if (item > 0)
                    {
                        listActe = (from a in ctx.ActeMedicals
                                    join tam in ctx.TraitementActeMedicals on a.Id equals tam.ActeMedicalId
                                    join t in ctx.Traitements on tam.TraitementId equals t.Id
                                    join med in ctx.Medecins on t.MedecinTraitantId equals med.Id
                                    where (a.Id == a1 || a.Id == a2 || a.Id == a3 || a.Id == a4 || a.Id == a5) && med.Id == item && t.DateTrait >= deb && t.DateTrait <= end
                                    select t).Union(
                                    from a in ctx.ActeMedicals
                                    join tam in ctx.TraitementActeMedicals on a.Id equals tam.ActeMedicalId
                                    join t in ctx.Traitements on tam.TraitementId equals t.Id
                                    join med in ctx.Medecins on t.MedecinRecommandantId equals med.Id
                                    where (a.Id == a1 || a.Id == a2 || a.Id == a3 || a.Id == a4 || a.Id == a5) && med.Id == item && t.DateTrait >= deb && t.DateTrait <= end
                                    select t).OrderBy(x => x.DateTrait).ToList();
                    }
                    else
                    {
                        (from a in ctx.ActeMedicals
                         join tam in ctx.TraitementActeMedicals on a.Id equals tam.ActeMedicalId
                         join t in ctx.Traitements on tam.TraitementId equals t.Id
                         where (a.Id == a1 || a.Id == a2 || a.Id == a3 || a.Id == a4 || a.Id == a5) && t.DateTrait >= deb && t.DateTrait <= end
                         select t).Union(
                                    from a in ctx.ActeMedicals
                                    join tam in ctx.TraitementActeMedicals on a.Id equals tam.ActeMedicalId
                                    join t in ctx.Traitements on tam.TraitementId equals t.Id
                                    where (a.Id == a1 || a.Id == a2 || a.Id == a3 || a.Id == a4 || a.Id == a5) && t.DateTrait >= deb && t.DateTrait <= end
                                    select t).OrderBy(x => x.DateTrait).ToList();
                    }

                    break;
                case 0:
                default:
                    if (item > 0)
                    {
                        listActe = (from t in ctx.Traitements
                                    join med in ctx.Medecins on t.MedecinTraitantId equals med.Id
                                    where med.Id == item && t.DateTrait >= deb && t.DateTrait <= end
                                    select t).Union(
                                    from t in ctx.Traitements
                                    join med in ctx.Medecins on t.MedecinRecommandantId equals med.Id
                                    where med.Id == item && t.DateTrait >= deb && t.DateTrait <= end
                                    select t).OrderBy(x => x.DateTrait).ToList();
                    }
                    else
                    {
                        listActe = ctx.Traitements.Where(x => x.DateTrait >= deb && x.DateTrait <= end).OrderBy(x => x.DateTrait).ToList();
                    }

                    break;
            }
            return listActe;
        }
	}
}