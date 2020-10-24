
using Microsoft.AspNet.Identity;
using Optica.Models;
using Optica.ViewModels;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Optica.Controllers
{
    [Authorize]
    public class ReglementFactureController : BaseController
    {

        private AppDbContext ctx = new AppDbContext();
        private static ReglementReport _reglementReport = new ReglementReport();
        //
        // GET: /ReglementFacture/
        public ActionResult Index()
        {
            return View();
        }

        [Route("règlement-fature/{id}/add/{origine?}",Name = "_addReglement")] //id du traitement
        public ActionResult Add(int id, string origine, Payement p)
        {
            var t = ctx.Traitements.Find(id);
            if (t != null && t.Del == false)
            {
                if (Request.HttpMethod == "POST")
                {
                    var p2 = new Payement { 
                        DateEnreg = DateTime.Now, 
                        DatePaye = new DateTime(p.DatePaye.Year, p.DatePaye.Month, p.DatePaye.Day), 
                        InfoCheque = p.InfoCheque, 
                        MontantPaye = p.MontantPaye, 
                        TraitementId = id, 
                        TypePaye = p.TypePaye 
                    };
                    if (!string.IsNullOrEmpty(origine)) //reglement assureur
                    {
                        p2.SourcePaye = Source.Assurance;
                        var prevPaye = t.Payements.Where(x => x.Del == false && x.SourcePaye == Source.Assurance).ToList().Sum(x => x.MontantPaye);
                        t.PayeAssur = t.PrixAssur == (p.MontantPaye + prevPaye) ? true : false;
                    }
                    else
                    {
                        //payement patient
                        p2.SourcePaye = Source.Patient;
                        t.MontantPaye += p.MontantPaye;
                        t.PayeOrd = t.MontantPaye == t.PrixOrd ? true : false;
                    }
                    ctx.Payements.Add(p2);
                    ctx.SaveChanges();
                    /*if (p2.SourcePaye == Source.Patient) //generate facture patient
                    {
                        return RedirectToAction("generateRecu", new { id = id, _url = "patient_Recu", _userLog = User.Identity.Name });
                    }*/
                }
                ViewBag.Origine = string.IsNullOrEmpty(origine) ? null : "assurance";
                return View(new Payement { DatePaye = DateTime.Now, Traitement = t });
            }
            return RedirectToAction("indexPatient", "Patient"); //A revoir
        }

        [Route("règlement-facture/{id}/{origine?}", Name = "_updateReglement")] // id du payement
        public ActionResult Update(int id, string origine, Payement p)
        {
            var p2 = ctx.Payements.Find(id);
            if (p2 != null && p2.Del == false)
            {
                if (Request.HttpMethod == "POST")
                {
                    var mtt = p2.MontantPaye;
                    p2.DatePaye = new DateTime(p.DatePaye.Year, p.DatePaye.Month, p.DatePaye.Day);
                    p2.InfoCheque = p.InfoCheque;
                    p2.MontantPaye = p.MontantPaye;
                    p2.TypePaye = p.TypePaye;
                    if (p2.SourcePaye == Source.Assurance)
                    {
                        var prevPaye = p2.Traitement.Payements.Where(x => x.Del == false && x.SourcePaye == Source.Assurance).ToList().Sum(x => x.MontantPaye) - p2.MontantPaye;
                        p2.Traitement.PayeAssur = p2.Traitement.PrixAssur == (prevPaye + p.MontantPaye) ? true : false;
                    }
                    else
                    {
                        p2.Traitement.MontantPaye -= mtt;
                        p2.Traitement.MontantPaye += p.MontantPaye;
                        p2.Traitement.PayeOrd = p2.Traitement.MontantPaye == p2.Traitement.PrixOrd ? true : false;
                    }
                    ctx.SaveChanges();
                    /*if (p2.SourcePaye == Source.Patient) //generate facture patient
                    {
                        return RedirectToAction("generateRecu", new { id = p2.TraitementId, _url = "patient_Recu", _userLog = User.Identity.Name });
                    }*/
                }
                ViewBag.Origine = string.IsNullOrEmpty(origine) ? null : "assurance";
                return View("Add", p2);
            }
            return RedirectToAction("indexPatient", "Patient"); 
        }

        [Route("reglement-facture/{id}/delete/", Name="_deletePayement")]
        public JsonResult delete (int id)
        {
            var payement = ctx.Payements.Find(id);
            if (payement == null)
                return Json(new { statut = "ERROR" }, JsonRequestBehavior.AllowGet);
            payement.Del = true;
            float mtt = 0;
            if (payement.SourcePaye == Source.Patient)
            {
                mtt = payement.Traitement.MontantPaye - payement.MontantPaye;
                var traitement = payement.Traitement;
                payement.Traitement.MontantPaye = mtt;
                payement.Traitement.PayeOrd = traitement.PrixOrd - mtt == 0 ? true : false;
            }
            else 
            {
                //surement regle par l'assureur faire
                var mttAss = ctx.Payements.Where(x => x.Del == false && x.SourcePaye == Source.Assurance && x.TraitementId == payement.TraitementId && x.Id != id).ToList().Sum(x => x.MontantPaye);
                mtt = payement.Traitement.PrixAssur - mttAss;
                payement.Traitement.PayeAssur = mtt == 0 ? true : false;
            }
            ctx.SaveChanges();
            return Json(new { statut = "OK"}, JsonRequestBehavior.AllowGet); 
        }

        [Route("règlement-facture/assureur/facture-à-regler/", Name="_listeFactureAssureurARegler")]
        public ActionResult factureAssureurARegler()
        {
            return View("factureAssureurARegler2");
        }

        [Route("règlement-facture/assureur/get-list-facture/", Name="_GetListFactureAssureur")]
        public JsonResult GetListFactureAssureur()
        {
            var listeFact = ctx.Traitements.Where(x => x.PayeAssur == false && x.AssuranceId != null && x.Del == false).OrderBy(x => x.Assurance.Nom).ToList();
            List<ModelFactureAssurance> listeFacture = new List<ModelFactureAssurance>();
            if (listeFact.Count > 0)
            {
                foreach (var item in listeFact)
                {
                    var refFacture = "Ref#" + item.DateTrait.ToString("dd"+ item.Id+"MM");
                    var identitePatient = item.DossierPat.Patient.Nom +" "+ item.DossierPat.Patient.Prenom;
                    var _resteAPayer = item.Payements.Where(x=> x.Del == false && x.SourcePaye == Source.Assurance).ToList().Sum(x=>x.MontantPaye);
                    _resteAPayer = item.PrixAssur - _resteAPayer;
                    listeFacture.Add(new ModelFactureAssurance { Id = item.Id, Charge = item.PrixAssur, Date = item.DateTrait, DateTraitement = item.DateTrait.ToString("dd/MM/yyyy"), IdentitePatient = identitePatient, NomAssurance = item.Assurance.Nom , RefFacture = refFacture, ResteApayer = _resteAPayer });
                }
            }
            return Json(listeFacture, JsonRequestBehavior.AllowGet);
        }

        [Route("règlement-facture/patient/facture-à-regler/", Name="_listeFacturePatientARegler")]
        public ActionResult facturePatientARegler()
        {     
            return View("facturePatientARegler2");
        }

        [Route("règlement-facture/patient/get-list-facture/", Name="_getListeFacturePatient")]
        public JsonResult GetListFacturePatient()
        {
            var listeTr = ctx.Traitements.Where(x => x.PayeOrd == false && x.Del == false).OrderByDescending(x => (x.PrixOrd - x.MontantPaye)).ToList();
            List<ModelFacturePatient> listFacture = new List<ModelFacturePatient>();
            if (listeTr.Count > 0)
            {
                foreach (var item in listeTr)
                {
                    var _refFacture = "Ref#" + item.DateTrait.ToString("dd"+item.Id+"MM");
                    var _identitePatient = item.DossierPat.Patient.Nom +" "+ item.DossierPat.Patient.Prenom;
                    var _resteApayer = item.PrixOrd - item.MontantPaye;
                    listFacture.Add(new ModelFacturePatient { Id = item.Id, Charge = item.PrixOrd, Date = item.DateTrait, DateTraitement = item.DateTrait.ToString("dd/MM/yyyy"), ResteApayer = _resteApayer, IdentitePatient = _identitePatient, RefFacture = _refFacture });
                }
            }
            return Json(listFacture,JsonRequestBehavior.AllowGet);
        }

        [Route("patient/liste-facture/{id}/", Name="_listeFacture")] //id du patient
        public ActionResult FacturesPatient(int id)
        { 
            var patient = ctx.Patients.Find(id);
            if (patient == null)
            {
                return new HttpNotFoundResult();
            }
            ViewBag.patient = patient;
            List<Traitement> ListeFact = new List<Traitement>();
            var _listeTraitement = from t in ctx.Traitements
                        join dp in ctx.DossierPats on t.DossierPatId equals dp.Id
                        where dp.PatientId == patient.Id && t.Del == false
                        select t;
            /*var dossierTr = ctx.DossierPats.Where(x => x.PatientId == id).Select(x => x.Id).ToList();
            foreach (var d in dossierTr)
            {
                var tr = ctx.Traitements.Where(x => x.DossierPatId == d).ToList();
                if (tr.Count > 0)
                {
                    foreach (var t in tr)
                    {
                        //var elt = new { idtr = t.Id, datetr = t.DateTrait, mtpat = t.PrixOrd, mtAss = t.PrixAssur, mtpatr = t.MontantPaye };
                        ListeFact.Add(t);
                    }
                }
            }*/
            //ViewBag.ListeFact = ListeFact.OrderByDescending(x => (x.PrixOrd - x.MontantPaye)).ToList();
            if (_listeTraitement.ToList().Count > 0)
            {
                //ViewBag.ListeFact = _listeTraitement.OrderByDescending(x => (x.PrixOrd - x.MontantPaye)).ToList();
                ViewBag.ListeFact = _listeTraitement.OrderByDescending(x => x.DateTrait).ToList();
            }
            else
            {
                ViewBag.ListeFact = ListeFact;
            }
            return View();
        }

        [Route("règlement-facture/reçu-patient/{id}/", Name = "_recuFacturePatient")] //id du paiement
        public ActionResult ApercuFacture(int id) 
        {
            var facture = ctx.Payements.Find(id);
            if (facture == null)
            {
                //return new HttpNotFoundResult();
                return HttpNotFound();
            }

            return View(facture);
        }

        [Route("reglement-facture/assurance/{id}", Name = "_factureAssurance")] //id du traitement
        public ActionResult ApercuFactureAssurance(int id)
        {
            var fact = ctx.Traitements.Find(id);
            if (fact == null)
            {
                return HttpNotFound();
            }

            return View(fact);
        }
        
        [AllowAnonymous]
        [Route("reglement-facture/Facture-assurance/{id}/", Name = "assurance_Fact")] //id du traitement url = reglement-facture/Facture-assurance/{id}/{_userLog}
        public ActionResult factureAssurance(int id) 
        {
            var t = ctx.Traitements.Find(id);
            if (t == null)
            {
                return HttpNotFound();
            }
            //_userLog = _userLog[0].ToString().ToUpper() + _userLog.Substring(1);
            var fact = new AssuranceFacture {
                  ActeMedicaux = t.ActeMedicaux.Where(x => x.Del == false).ToList(),
                  Agence = "Assivito (Lomé)",
                  Caisse = "",//_userLog,
                  Date = t.DateTrait,
                  Montant = t.PrixAssur,
                  NumRecu = t.DateTrait.ToString("dd" + t.Id +"MM"),
                  patient = t.DossierPat.Patient,
                  Assurance = t.Assurance, 
                  MontantPatient = t.PrixOrd
            };
            return View(fact);
        }

        [AllowAnonymous]
        [Route("reglement-facture/facture-patient/{id}/", Name = "patient_Recu")] //id du traitement url = reglement-facture/facture-patient/{id}/{_userLog}
        public ActionResult FacturePatient(int id) 
        {
            var t = ctx.Traitements.Find(id);
            if (t == null)
            {
                return HttpNotFound();
            }
            Payement p = t.Payements.Where(x => x.Del == false && x.SourcePaye == Source.Patient).OrderByDescending(x => x.Id).FirstOrDefault();
            //_userLog = _userLog[0].ToString().ToUpper() + _userLog.Substring(1);
            var fact = new PatientFacture
            {
                ActeMedicaux  = t.ActeMedicaux.Where(x => x.Del == false).ToList(),
                Agence = "Assivito (Lomé)",
                Assurance = t.Assurance,
                Caisse = "",//_userLog,
                Date = t.DateTrait,
                MontantAssurance = t.PrixAssur,
                MontantGlobale = t.PrixAssur + t.PrixOrd,
                MontantPatient = t.PrixOrd,
                NumRecu = p.DatePaye.ToString("dd" + p.Id + "MM"),
                Patient = t.DossierPat.Patient,
                Payement = p,
                Payements = t.Payements.Where(x => x.Del == false && x.SourcePaye == Source.Patient).ToList(),
                ResteApayer = t.PrixOrd - t.MontantPaye
            };
            fact.StatutReglement = fact.ResteApayer == 0 ? "Soldé" : "Partiellement soldé";
            return View(fact);
        }

        [Route("administration/journal-caisse/", Name="_journalReglementFacture")]
        [HttpGet]
        public ActionResult ListeFactureRegle()
        {
            ViewBag.deb = DateTime.Now;
            return View();
        }


        [Route("administration/journal-caisse/query/", Name = "_ListeFactureRegle")]
        public JsonResult ListeReglement(DateTime? deb, DateTime? end, string print)
        {         
            List<ReglementCaisse> rc = new List<ReglementCaisse>();
            List<Payement> listeFactureRegle = new List<Payement>();
            List<object> resultat = new List<object>();
            string ddeb, dend;
            if (deb != null && end != null)
            {
                listeFactureRegle = ctx.Payements.Where(x => x.DatePaye >= deb && x.DatePaye <= end).OrderBy(x=>x.DatePaye).ToList();
                ddeb = deb.Value.ToString("dd/MM/yyyy");
                dend = end.Value.ToString("dd/MM/yyyy");
            }
            else if(deb != null && end == null)
            {
                listeFactureRegle = ctx.Payements.Where(x => x.DatePaye > deb).OrderBy(x=>x.DatePaye).ToList();
                ddeb = deb.Value.ToString("dd/MM/yyyy");
                dend = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else if (deb == null && end != null)
            {
                listeFactureRegle = ctx.Payements.Where(x => x.DatePaye <= end).OrderBy(x=>x.DatePaye).ToList();
                var y = ctx.Patients.OrderBy(x => x.Id).FirstOrDefault();
                ddeb = y != null ? y.DateEnreg.ToString("dd/MM/yyyy") : " depuis debut operation";
                dend = end.Value.ToString("dd/MM/yyyy");
            }
            else
            {
                listeFactureRegle = ctx.Payements.OrderByDescending(x => x.Id).OrderBy(x=>x.DatePaye).ToList();
                var y = ctx.Patients.OrderBy(x => x.Id).FirstOrDefault();
                ddeb = y != null ? y.DateEnreg.ToString("dd/MM/yyyy") : " depuis debut operation";
                dend = DateTime.Now.ToString("dd/MM/yyyy");
            }
            float montant = 0;
            if (listeFactureRegle.Count > 0)
            {   
                foreach (var elt in listeFactureRegle)
                {
                    montant += elt.Del ? 0 : elt.MontantPaye;
                    var p = elt.Traitement.DossierPat.Patient;
                    var _montantR = elt.MontantPaye.ToString("N0",CultureInfo.CurrentCulture);
                    string nomPatient = p.Nom + " "+ p.Prenom;
                    rc.Add(
                        new ReglementCaisse { 
                            Date = elt.DatePaye,
                            DateT = elt.DatePaye.ToString("dd/MM/yyyy"),
                            ModeReglement = elt.TypePaye,
                            Montant = elt.Del ? "-"+_montantR : _montantR,
                            NomPatient = nomPatient,
                            SourceReglement = elt.SourcePaye.ToString(),
                            NumFacture = elt.DatePaye.ToString("dd" + elt.Id + "MM")//elt.Traitement.DateTrait.ToString("dd" + elt.TraitementId + "MM") 
                        });
                }
            }
            if (!string.IsNullOrEmpty(print))
            {
                //generer un fichier à imprimer
                string state = "ERROR";
                if (rc.Count > 0)
                {
                    var _rc = new ReglementReport { DateDebut = ddeb, DateFin = dend, MontantReport = montant, Rc = rc };
                    _reglementReport = _rc;
                    state = generateReport("_journalCaisseReport"); 
                }   
                return Json(new { qState = state }, JsonRequestBehavior.AllowGet);
            }
            resultat.Add(new { Mtt = montant.ToString("N0", CultureInfo.CurrentCulture), fact = rc});
            return Json(resultat, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        [Route("administration/journal-de-caisse/EtatCaisse/", Name="_journalCaisseReport")]
        public ActionResult CaisseReport()
        {
            return View(_reglementReport);
        }

        [Route("print/temp-facture/", Name = "_factureGenerator")] //url = print/{_url}/{id}/{_userLog}
        public JsonResult generateRecu(string _url, int id, string source) 
        {
            var t = ctx.Traitements.Find(id);
            var p = t.DossierPat.Patient;
            //var p3 = t.Payements.OrderByDescending(x => x.Id).FirstOrDefault();

            var url = /*_url + id;*/ Url.RouteUrl(_url, new { id = id});
            url = Request.Url.Authority + url;
            //new Converter
            HtmlToPdf Converter = new HtmlToPdf();
            //new document
           

            string filename = "factureassurance.pdf";//"Fact" + t.DateTrait.ToString("dd" + id + "MM") + ".pdf";
            if (source == "patientRecu")
            {
                /*var payement = t.Payements.OrderByDescending(x => x.Id).FirstOrDefault();
                filename = "Factp" + payement.DatePaye.ToString("dd"+ payement.Id+"MM") + ".pdf";*/
                filename = "recupatient.pdf";
            }

            string statut = "ERROR";
            try
            {
                PdfDocument doc = Converter.ConvertUrl(url);
                string saveDir = Server.MapPath(System.IO.Path.Combine("~/Datadir/", filename));
                //saveDocument
                doc.Save(saveDir);
                doc.Close();
                statut = "OK";
            } catch (Exception e) {
                //
            }
            //trouver le patient correspondant au traitement
            //var p = ctx.Traitements.Where(x => x.Id == id).Select(x => x.DossierPat.Patient).FirstOrDefault();
            /*if (_url == "patient_Recu")
                return RedirectToAction("Add", new { id = id});*/
            return Json(new { Result = statut, filename = filename }, JsonRequestBehavior.AllowGet);//RedirectToAction("Index", "TraitementOrd", new { id = p.Id });//View(fact);
        }

        /*private string generateReport(string _url)
        {
            var url = Url.RouteUrl(_url);
            url = Request.Url.Authority + url;
            //new Converter
            HtmlToPdf Converter = new HtmlToPdf();
            //document parameters
            var enteteUrl = Server.MapPath("~/Views/Shared/EntetePartial.html");
            var footerUrl = Server.MapPath("~/Views/Shared/FooterPartial.html");
            //var footerUrl = Server.MapPath("~/Datadir/FooterPartial.html");
            //Converter.Options.CssMediaType = 
            //header params
            Converter.Options.DisplayHeader = true;
            Converter.Header.DisplayOnFirstPage = true;
            Converter.Header.DisplayOnEvenPages = true;
            Converter.Header.DisplayOnOddPages = true;
            Converter.Header.Height = 95;
            PdfHtmlSection headerHtml = new PdfHtmlSection(enteteUrl);
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            Converter.Header.Add(headerHtml);
           
            //footer params
            Converter.Options.DisplayFooter = true;
            Converter.Footer.DisplayOnFirstPage = true;
            Converter.Footer.DisplayOnOddPages = true;
            Converter.Footer.DisplayOnEvenPages = true;
            Converter.Footer.Height = 20;
            PdfHtmlSection footerhtml = new PdfHtmlSection(footerUrl);
            footerhtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            Converter.Footer.Add(footerhtml);
            //add page number element to footer
            PdfTextSection text = new PdfTextSection(0,4,"Page: {page_number} sur {total_pages} ",new System.Drawing.Font("Helvetica",8));
            text.HorizontalAlign = PdfTextHorizontalAlign.Center;
            PdfTextSection text2 = new PdfTextSection(0,4,"Lomé,"+DateTime.Now.ToString("dd/MM/yyyy"), new System.Drawing.Font("Helvetica",8));
            text2.HorizontalAlign = PdfTextHorizontalAlign.Right;
            
            Converter.Footer.Add(text);
            //Converter.Footer.Add(text2);
            //conversion delay
            Converter.Options.MinPageLoadTime = 2;
            Converter.Options.MaxPageLoadTime = 90;
            Converter.Options.PdfPageSize = PdfPageSize.A4;
            Converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            Converter.Options.MarginTop = 30;
            Converter.Options.MarginBottom = 20;
            Converter.Options.MarginRight = 10;
            //new document
            string statut = "ERROR";
            try
            {
                PdfDocument doc = Converter.ConvertUrl(url);
                string filename = "caisseReport.pdf";
                string saveDir = Server.MapPath(System.IO.Path.Combine("~/Datadir/", filename));
                //saveDocument
                doc.Save(saveDir);
                doc.Close();
                statut = "OK";
            }
            catch (Exception e)
            {

            }
           
            return statut;
        }*/
	}
}