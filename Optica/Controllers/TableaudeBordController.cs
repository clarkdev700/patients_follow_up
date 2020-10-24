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
    public class TableaudeBordController : Controller
    {
        private AppDbContext ctx = new AppDbContext();  

        [Route("dashboard/", Name="_dashboard")]
        public ActionResult Index()
        {
            //patient enregistre
            var Deb = DateTime.Now.Date;
            var End = DateTime.Now.AddDays(1).Date;
            var regPatient = ctx.Patients.Where(x => x.DateEnreg >= Deb && x.DateEnreg < End && x.Del == false).ToList().Count();
            var caisse = ctx.Payements.Where(x => x.DateEnreg >= Deb && x.DateEnreg < End && x.Del == false).Select(x => x.MontantPaye).ToList().Sum();
            var consultation = from t in ctx.Traitements
                               join at in ctx.TraitementActeMedicals on t.Id equals at.TraitementId
                               join am in ctx.ActeMedicals on at.ActeMedicalId equals am.Id
                               where am.Code.ToUpper() == "CS" && (t.DateTrait >= Deb && t.DateTrait < End) && t.Del == false && am.Del == false
                               select t;
            return View(new DashViewModel { RegPatient = regPatient, Cash = caisse, RegConsultation = consultation.Count() });
        }

        [Route("dashboard/analyses-stat/", Name ="_dashboardAnalyses")]
        public JsonResult AnalyseStat()
        {
            var Deb = DateTime.Now.Date;
            var End = DateTime.Now.AddDays(1).Date;
            var anlyseStat = new List<AnalyseStats>();
            int total = 0; //double _nbre = 0;
            var acteMedId = ctx.ActeMedicals.Where(x => x.Del == false && x.Code.ToUpper() != "CS").Select(x => new { x.Id, x.Code}).Distinct().ToList();
            if (acteMedId.Count > 0)
            {
                foreach (var x in acteMedId) 
                {
                    //recuperer les traitement correspondant à l'acte
                    var nb = from t in ctx.Traitements
                             join at in ctx.TraitementActeMedicals on t.Id equals at.TraitementId
                             where at.Del == false && (t.DateTrait >= Deb && t.DateTrait < End) && t.Del == false && at.ActeMedicalId == x.Id
                             select t;
                    total += nb.Count();
                    anlyseStat.Add(new AnalyseStats { Acte = x.Code, Qte = nb.Count()});
                }
                foreach (var item in anlyseStat)
                {
                    //_nbre = (item.Qte / total);
                    item.Vpourcent = total > 0 ? Math.Round(((double)item.Qte / total),2,MidpointRounding.AwayFromZero) * 100 : 0;
                }
            } 
            return Json(anlyseStat, JsonRequestBehavior.AllowGet);
        }

        [Route("dashboard/dette-stat", Name ="__dashboardDette")]
        public JsonResult DetteStat()
        {
            //patient dette
            var pdette = ctx.Traitements.Where(x => x.PayeOrd == false && x.Del == false).ToList();
            float mttPatient = 0, mttAss = 0;
            foreach (var item in pdette)
            {
                mttPatient += (item.PrixOrd - item.MontantPaye);
            }
            var  assdette= ctx.Traitements.Where(x => x.PayeAssur == false && x.AssuranceId != null && x.Del == false).ToList();
            foreach (var item in assdette)
            {
                mttAss += (item.PrixAssur - item.Payements.Where(x => x.Del == false && x.SourcePaye == Source.Assurance).ToList().Sum(x => x.MontantPaye));
            }
            var detteStat = new List<DetteStats>();
            var tot = mttPatient + mttAss;
            detteStat.Add(new DetteStats { Origine = "Patient", Montant = mttPatient, VPourcent = tot > 0 ? Math.Round(((double)mttPatient / tot),3) * 100 : 0});
            detteStat.Add(new DetteStats { Origine = "Assurance", Montant = mttAss, VPourcent = tot > 0 ? Math.Round(((double)mttAss / tot),3) * 100 : 0});
            return Json(detteStat, JsonRequestBehavior.AllowGet);
        }

        [Route("dashboard/analyse-week-stat/", Name="_dashboardAnalyseWeekStat")]
        public JsonResult AnalyseWeekStat()
        {
            var mday = DateTime.Now;
            var dplage = new List<datePlage>();
            switch (mday.ToString("ddd"))
            {
                case "lun.":
                    dplage.Add(new datePlage { Jour = "Lun", Deb = mday.Date, End = mday.AddDays(1).Date });
                    dplage.Add(new datePlage { Jour = "Mar", Deb = null /*mday.AddDays(1).Date*/, End = null /*mday.AddDays(2).Date*/});
                    dplage.Add(new datePlage { Jour = "Mer", Deb = null /*mday.AddDays(2).Date*/, End = null /*mday.AddDays(3).Date*/ });
                    dplage.Add(new datePlage { Jour = "Jeu", Deb = null /*mday.AddDays(3).Date*/, End = null /*mday.AddDays(4).Date*/ });
                    dplage.Add(new datePlage { Jour = "Ven", Deb = null /*mday.AddDays(4).Date*/, End = null /*mday.AddDays(5).Date*/ });
                    dplage.Add(new datePlage { Jour = "Sam", Deb = null /*mday.AddDays(5).Date*/, End = null /*mday.AddDays(6).Date*/ });
                    break;
                case "mar.":
                    dplage.Add(new datePlage { Jour = "Lun", Deb = mday.AddDays(-1).Date, End = mday.Date });
                    dplage.Add(new datePlage { Jour = "Mar", Deb = mday.Date, End = mday.AddDays(1).Date});
                    dplage.Add(new datePlage { Jour = "Mer", Deb = null /*mday.AddDays(1).Date*/, End = null /*mday.AddDays(2).Date*/ });
                    dplage.Add(new datePlage { Jour = "Jeu", Deb = null /*mday.AddDays(2).Date*/, End = null /*mday.AddDays(3).Date*/ });
                    dplage.Add(new datePlage { Jour = "Ven", Deb = null /*mday.AddDays(3).Date*/, End = null /*mday.AddDays(4).Date*/ });
                    dplage.Add(new datePlage { Jour = "Sam", Deb = null /*mday.AddDays(4).Date*/, End = null/*mday.AddDays(5).Date*/ });
                    break;
                case "mer.":
                    dplage.Add(new datePlage { Jour = "Lun", Deb = mday.AddDays(-2).Date, End = mday.AddDays(-1).Date });
                    dplage.Add(new datePlage { Jour = "Mar", Deb = mday.AddDays(-1).Date, End = mday.Date});
                    dplage.Add(new datePlage { Jour = "Mer", Deb = mday.Date, End = mday.AddDays(1).Date });
                    dplage.Add(new datePlage { Jour = "Jeu", Deb = null/*mday.AddDays(1).Date*/, End = null /*mday.AddDays(2).Date*/ });
                    dplage.Add(new datePlage { Jour = "Ven", Deb = null/*mday.AddDays(2).Date*/, End = null/*mday.AddDays(3).Date*/ });
                    dplage.Add(new datePlage { Jour = "Sam", Deb = null/*mday.AddDays(3).Date*/, End = null/*mday.AddDays(4).Date*/ });
                    break;
                case "jeu.":
                    dplage.Add(new datePlage { Jour = "Lun", Deb = mday.AddDays(-3).Date, End = mday.AddDays(-2).Date });
                    dplage.Add(new datePlage { Jour = "Mar", Deb = mday.AddDays(-2).Date, End = mday.AddDays(-1).Date});
                    dplage.Add(new datePlage { Jour = "Mer", Deb = mday.AddDays(-1).Date, End = mday.Date });
                    dplage.Add(new datePlage { Jour = "Jeu", Deb = mday.Date, End = mday.AddDays(1).Date });
                    dplage.Add(new datePlage { Jour = "Ven", Deb = null /*mday.AddDays(1).Date*/, End = null/*mday.AddDays(2).Date*/ });
                    dplage.Add(new datePlage { Jour = "Sam", Deb = null/*mday.AddDays(2).Date*/, End = null/*mday.AddDays(3).Date*/ });
                    break;
                case "ven.":
                    dplage.Add(new datePlage { Jour = "Lun", Deb = mday.AddDays(-4).Date, End = mday.AddDays(-3).Date });
                    dplage.Add(new datePlage { Jour = "Mar", Deb = mday.AddDays(-3).Date, End = mday.AddDays(-2).Date});
                    dplage.Add(new datePlage { Jour = "Mer", Deb = mday.AddDays(-2).Date, End = mday.AddDays(-1).Date });
                    dplage.Add(new datePlage { Jour = "Jeu", Deb = mday.AddDays(-1).Date, End = mday.Date });
                    dplage.Add(new datePlage { Jour = "Ven", Deb = mday.Date, End = mday.AddDays(1).Date });
                    dplage.Add(new datePlage { Jour = "Sam", Deb = null/*mday.AddDays(1).Date*/, End = null/*mday.AddDays(2).Date*/ });
                    break;
                case "sam.":
                    dplage.Add(new datePlage { Jour = "Lun", Deb = mday.AddDays(-5).Date, End = mday.AddDays(-4).Date });
                    dplage.Add(new datePlage { Jour = "Mar", Deb = mday.AddDays(-4).Date, End = mday.AddDays(-3).Date});
                    dplage.Add(new datePlage { Jour = "Mer", Deb = mday.AddDays(-3).Date, End = mday.AddDays(-2).Date });
                    dplage.Add(new datePlage { Jour = "Jeu", Deb = mday.AddDays(-2).Date, End = mday.AddDays(-1).Date });
                    dplage.Add(new datePlage { Jour = "Ven", Deb = mday.AddDays(-1).Date, End = mday.Date });
                    dplage.Add(new datePlage { Jour = "Sam", Deb = mday.Date, End = mday.AddDays(1).Date });
                    break;
            }
            //recuperation des analyses
            var analyses = ctx.ActeMedicals.Where(x => x.Del == false && x.Code.ToUpper() != "CS").Select(x => new { Id = x.Id, Code = x.Code }).OrderBy(x => x.Code).ToList();
            var acteGroup = new List<DayGroupStat>();
            var _acteList = new List<AnalyseStats>();
            foreach (var item in dplage)
            {
                _acteList.Clear();
                foreach (var a in analyses)
                {
                    if (item.Deb != null)
                    {
                        var _actes = from t in ctx.Traitements
                                     join at in ctx.TraitementActeMedicals on t.Id equals at.TraitementId
                                     where at.Del == false && at.ActeMedicalId == a.Id && (t.DateTrait >= item.Deb.Value && t.DateTrait < item.End.Value) && t.Del == false
                                     select t;
                        _acteList.Add(new AnalyseStats { Acte = a.Code, Qte = _actes.Count() });
                    }
                    else
                    {
                        _acteList.Add(new AnalyseStats { Acte = a.Code, Qte = 0 });
                    } 
                }
                acteGroup.Add(new DayGroupStat { Jour = item.Jour, V1 = _acteList[0], V2 = _acteList[1], V3 = _acteList[2], V4 = _acteList[3]/*, V5 = _acteList[4]*/}); 
            }
           
            return Json(acteGroup, JsonRequestBehavior.AllowGet);
        }

        [Route("dashboard/assurance-dette-stat/", Name="_dashboardAssuranceStat")]
        public JsonResult AssuranceStat()
        {
            //list des assurances
            List<AssurDette> _LassurDette = new List<AssurDette>();
            List<AssurDette> AssuranceDette = new List<AssurDette>();
            var listeTraitement = ctx.Traitements.Where(x => x.AssuranceId != null && x.PayeAssur == false && x.PrixAssur > 0 && x.Del == false).ToList();
            if (listeTraitement.Count > 0)
            {
                foreach (var t in listeTraitement)
                {
                    var mttRegele = t.Payements.Where(x => x.Del == false && x.SourcePaye == Source.Assurance).ToList().Sum(x => x.MontantPaye);
                    var _resteApayer = t.PrixAssur - mttRegele;
                    _LassurDette.Add(new AssurDette { CodeAssur = t.Assurance.Code, Montant = _resteApayer, NomAssur = t.Assurance.Nom });
                }
            }
            var temp = _LassurDette.GroupBy(x => x.CodeAssur).ToList();

            double mttTotal = 0;
            if (temp.Count > 0)
            {
                mttTotal = _LassurDette.Sum(x => x.Montant);
                foreach (var item in temp)
                {
                    var assurance = item.FirstOrDefault();
                    var mttAssur = item.Sum(x=>x.Montant); 
                    var taux = (mttAssur * 100) / mttTotal;
                    AssuranceDette.Add(new AssurDette { CodeAssur = assurance.CodeAssur, NomAssur = assurance.NomAssur, Montant = mttAssur, Taux = Math.Round(taux, 2) });
                }
            }

            /*var _lAssur = ctx.Assurances.Where(x => x.Del == false).ToList();
            if (_lAssur.Count > 0)
            {
                foreach (var assur in _lAssur)
                {
                    //recuperation des traitements
                    var _traitementMtt = ctx.Traitements.Where(x => x.AssuranceId == assur.Id && x.Del == false).ToList().Sum(x => x.PrixAssur);
                    var _traitementMttRegl = from p in ctx.Payements
                                             join t in ctx.Traitements on p.TraitementId equals t.Id
                                             where p.Del == false && p.SourcePaye == Source.Assurance && t.AssuranceId == assur.Id && t.Del == false
                                             select p.MontantPaye;
                    var _mttApayer = _traitementMtt - _traitementMttRegl.ToList().Sum();
                    if (_mttApayer > 0)
                    {
                        _LassurDette.Add(new AssurDette { NomAssur = assur.Nom, CodeAssur = assur.Code ,Montant = _mttApayer });
                    }
                }
            }*/
            return Json(AssuranceDette/*_LassurDette*/, JsonRequestBehavior.AllowGet);
        }
	}
}